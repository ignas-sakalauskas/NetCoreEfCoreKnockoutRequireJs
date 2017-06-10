define(["components/client.viewmodel", "knockout", "./client.viewmodel-test-data"],
    function (clientViewModel) {
        describe("Client View Model", function () {

            var datasetserviceSpy;
            var formattingServiceSpy;
            var categoriesServiceSpy;

            beforeEach(function () {
                // return promise mock
                var d = $.Deferred();
                d.resolve({}); // make data service return success           

                // data service mock
                datasetserviceSpy = jasmine.createSpyObj("datasetserviceSpy", ["getItem", "createItem", "updateItem", "deleteItem"]);
                datasetserviceSpy.getItem.and.returnValue(d);

                formattingServiceSpy = jasmine.createSpyObj("formattingServiceSpy", ["formatDate"]);

                var dd = $.Deferred();
                dd.resolve([]);
                categoriesServiceSpy = jasmine.createSpyObj("categoriesServiceSpy", ["getCategories"]);
                categoriesServiceSpy.getCategories.and.returnValue(dd);
            });

            afterEach(function () {
                datasetserviceSpy = null;
                formattingServiceSpy = null;
                categoriesServiceSpy = null;
            });

            it("Error should be empty by default.", function () {
                // Arrange
                // Act
                var self = createViewModelForTesting();

                // Assert
                expect(self.error().length).toBe(0);
            });

            it("editEnabled and showError should be false by default.", function () {
                // Arrange
                // Act
                var self = createViewModelForTesting();

                // Assert
                expect(self.editEnabled()).toBe(false);
                expect(self.showError()).toBe(false);
            });

            it("model should be empty by default.", function () {
                // Arrange
                // Act
                var self = createViewModelForTesting();

                // Assert
                expect(self.clientId()).toBe(0);
                expect(self.name()).toBe("");
                expect(self.email()).toBe("");
                expect(self.phone()).toBe("");
                expect(self.fax()).toBe("");
                expect(self.address()).toBe("");
                expect(self.status()).toBe(0);
                expect(self.createdOn()).toBe("");
                expect(self.categoryId()).toBe(0);
            });

            it("params should be assigned to view model.", function () {
                // Arrange
                // Act
                var self = createViewModelForTesting(paramsTestCase);

                // Assert
                expect(self.clientId()).toBe(1);
                expect(self.listLink).toBe("list");
                expect(self.mode).toBe("mode");
                expect(self.clientStatusEnum).toEqual({ Item: 1 });
            });

            it("isAddMode should be true when mode is Add.", function () {
                // Arrange
                var params = { mode: "Add" };

                // Act
                var self = createViewModelForTesting(params);

                // Assert
                expect(self.isAddMode).toBe(true);
            });

            it("isEditMode should be true when mode is Edit.", function () {
                // Arrange
                var params = { mode: "Edit" };

                // Act
                var self = createViewModelForTesting(params);

                // Assert
                expect(self.isEditMode).toBe(true);
            });

            it("statuses should be assigned array of enum items.", function () {
                // Arrange
                var params = { clientStatusEnum: { Item1: 1, Item2: 2 } };

                // Act
                var self = createViewModelForTesting(params);

                // Assert
                expect(self.statuses()).toEqual([{ id: 1, name: "Item1" }, { id: 2, name: "Item2" }]);
            });

            it("pageTitle should depend on isAddMode, isEditMode, and model name.", function () {
                pageTitleTestCases.forEach(function (test) {
                    // Arrange
                    // Act
                    var self = createViewModelForTesting();
                    self.isAddMode = test.add;
                    self.isEditMode = test.edit;
                    spyOn(self, "name").and.returnValue(test.name);

                    // Assert
                    expect(self.pageTitle()).toBe(test.expected);
                });
            });

            it("editLabel should depend on editEnabled.", function () {
                editLabelTestCases.forEach(function (test) {
                    // Arrange
                    // Act
                    var self = createViewModelForTesting();
                    spyOn(self, "editEnabled").and.returnValue(test.enabled);

                    // Assert
                    expect(self.editLabel()).toBe(test.expected);
                });
            });

            it("showError should be true when error is not empty.", function () {
                // Arrange
                var self = createViewModelForTesting();

                // Act
                self.error("1");

                // Assert
                expect(self.showError()).toBe(true);
            });

            it("canSave should depend on editEnabled and isValid.", function () {
                canSaveTestCases.forEach(function (test) {
                    // Arrange
                    // Act
                    var self = createViewModelForTesting();
                    spyOn(self, "editEnabled").and.returnValue(test.enabled);
                    spyOn(self, "isValid").and.returnValue(test.valid);

                    // Assert
                    expect(self.canSave()).toBe(test.expected);
                });
            });

            it("editEnabled should be true when Add mode is active.", function () {
                // Arrange
                // Act
                var params = { mode: "add" };
                var self = createViewModelForTesting(params);

                // Assert
                expect(self.editEnabled()).toBe(true);
            });

            it("enableEdit should invert editEnabled value.", function () {
                // Arrange
                var self = createViewModelForTesting();
                self.editEnabled(true);

                // Act
                self.enableEdit();

                // Assert
                expect(self.editEnabled()).toBe(false);
            });

            it("load should always call categoriesService.getCategories on start.", function () {
                // Arrange
                // Act
                var self = createViewModelForTesting();

                // Assert
                expect(self.categoriesService.getCategories).toHaveBeenCalled();
            });

            it("load should call loadClient with clientId on start when categoriesService.getCategories() promise resolved and Edit mode is active.", function () {
                // Arrange
                var self = createViewModelForTesting();
                self.clientId(1);
                self.isEditMode = true;
                spyOn(self, "loadClient");

                // Act
                self.load();

                // Assert
                expect(self.loadClient).toHaveBeenCalledWith(self.clientId());
            });

            it("load should not call loadClient on start when categoriesService.getCategories() promise resolved and Edit mode is not active.", function () {
                // Arrange
                var self = createViewModelForTesting();
                self.clientId(1);
                self.isAddMode = true;
                spyOn(self, "loadClient");

                // Act
                self.load();

                // Assert
                expect(self.loadClient).not.toHaveBeenCalled();
            });

            it("loadClient should set error with responseText when dataService.getItem promise rejected.", function () {
                // Arrange
                var d = $.Deferred();
                d.reject({ responseText: "test" });
                datasetserviceSpy.getItem.and.returnValue(d);
                var self = createViewModelForTesting();
                self.clientId(1);
                self.isEditMode = true;

                // Act
                self.loadClient(1);

                // Assert
                expect(self.error().length).toBeGreaterThan(0);
                expect(self.error().indexOf("test")).toBeGreaterThan(0);
            });

            it("loadClient should set model properties with response when dataService.getItem promise resolved.", function () {
                // Arrange
                var d = $.Deferred();
                d.resolve(modelTestObject);
                datasetserviceSpy.getItem.and.returnValue(d);
                var self = createViewModelForTesting();
                self.clientId(1);
                self.isEditMode = true;

                // Act
                self.loadClient(1);

                // Assert
                expect(self.name()).toBe("name");
                expect(self.email()).toBe("email");
                expect(self.phone()).toBe("phone");
                expect(self.fax()).toBe("fax");
                expect(self.address()).toBe("address");
                expect(self.status()).toBe("status");
                expect(self.categoryId()).toBe(2);
            });

            it("loadClient should set model createdOn with response using formatting service when dataService.getItem promise resolved.", function () {
                // Arrange
                var d = $.Deferred();
                d.resolve({ createdOn: "2017" });
                datasetserviceSpy.getItem.and.returnValue(d);
                formattingServiceSpy.formatDate.and.returnValue("2017");
                var self = createViewModelForTesting();
                self.clientId(1);
                self.isEditMode = true;

                // Act
                self.loadClient(1);

                // Assert
                expect(self.createdOn()).toBe("2017");
            });

            it("deleteClick should set error with responseText when dataService.deleteItem promise rejected.", function () {
                // Arrange
                var d = $.Deferred();
                d.reject({ responseText: "test" });
                datasetserviceSpy.deleteItem.and.returnValue(d);
                var self = createViewModelForTesting();
                self.clientId(1);
                self.isEditMode = true;

                // Act
                self.deleteClick();

                // Assert
                expect(self.dataService.deleteItem).toHaveBeenCalledWith(self.clientId());
                expect(self.error().length).toBeGreaterThan(0);
                expect(self.error().indexOf("test")).toBeGreaterThan(0);
            });

            it("deleteClick should set model properties with response when dataService.deleteItem promise resolved.", function () {
                // Arrange
                var d = $.Deferred();
                d.resolve({});
                datasetserviceSpy.deleteItem.and.returnValue(d);
                var self = createViewModelForTesting();
                self.clientId(1);
                spyOn(self, "redirectToList");

                // Act
                self.deleteClick();

                // Assert
                expect(self.dataService.deleteItem).toHaveBeenCalledWith(self.clientId());
                expect(self.redirectToList).toHaveBeenCalled();
            });

            it("createClient should set error with responseText when dataService.createItem promise rejected.", function () {
                // Arrange
                var obj = { name: "name" };
                var d = $.Deferred();
                d.reject({ responseText: "test" });
                datasetserviceSpy.createItem.and.returnValue(d);
                var self = createViewModelForTesting();

                // Act
                self.createClient(obj);

                // Assert
                expect(self.dataService.createItem).toHaveBeenCalledWith(obj);
                expect(self.error().length).toBeGreaterThan(0);
                expect(self.error().indexOf("test")).toBeGreaterThan(0);
            });

            it("createClient should set call redirectToList when dataService.deleteItem promise resolved.", function () {
                // Arrange
                var obj = { name: "name" };
                var d = $.Deferred();
                d.resolve({});
                datasetserviceSpy.createItem.and.returnValue(d);
                var self = createViewModelForTesting();
                spyOn(self, "redirectToList");

                // Act
                self.createClient(obj);

                // Assert
                expect(self.dataService.createItem).toHaveBeenCalledWith(obj);
                expect(self.redirectToList).toHaveBeenCalled();
            });

            it("updateClient should set error with responseText when dataService.updateItem promise rejected.", function () {
                // Arrange
                var obj = { name: "name" };
                var d = $.Deferred();
                d.reject({ responseText: "test" });
                datasetserviceSpy.updateItem.and.returnValue(d);
                var self = createViewModelForTesting();
                spyOn(self, "clientId").and.returnValue(1);

                // Act
                self.updateClient(obj);

                // Assert
                expect(self.dataService.updateItem).toHaveBeenCalledWith(1, obj);
                expect(self.error().length).toBeGreaterThan(0);
                expect(self.error().indexOf("test")).toBeGreaterThan(0);
            });

            it("updateClient should set call redirectToList when dataService.updateItem promise resolved.", function () {
                // Arrange
                var obj = { name: "name" };
                var d = $.Deferred();
                d.resolve({});
                datasetserviceSpy.updateItem.and.returnValue(d);
                var self = createViewModelForTesting();
                spyOn(self, "clientId").and.returnValue(1);
                spyOn(self, "redirectToList");

                // Act
                self.updateClient(obj);

                // Assert
                expect(self.dataService.updateItem).toHaveBeenCalledWith(1, obj);
                expect(self.redirectToList).toHaveBeenCalled();
            });

            it("saveClick should call createClient with model when Add mode active.", function () {
                // Arrange
                var self = createViewModelForTesting();
                self.isAddMode = true;
                setTestModelValues(self);
                spyOn(self, "createClient");

                // Act
                self.saveClick();

                // Assert
                expect(self.createClient).toHaveBeenCalledWith(modelTestObject);
            });

            it("saveClick should call createClient with model when Add mode active.", function () {
                // Arrange
                var self = createViewModelForTesting();
                self.isEditMode = true;
                setTestModelValues(self);
                spyOn(self, "updateClient");

                // Act
                self.saveClick();

                // Assert
                expect(self.updateClient).toHaveBeenCalledWith(modelTestObject);
            });

            // private helper
            function createViewModelForTesting(paramsSpy) {
                var params = paramsSpy || { clientStatusEnum: {} };

                var self = new clientViewModel(params, datasetserviceSpy, formattingServiceSpy, categoriesServiceSpy);

                return self;
            }
        });
    }
);
