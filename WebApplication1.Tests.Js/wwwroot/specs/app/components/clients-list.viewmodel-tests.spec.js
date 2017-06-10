define(["components/clients-list.viewmodel", "knockout", "./clients-list.viewmodel-test-data"],
    function (clientsListViewModel) {
        describe("Clients List View Model", function () {

            var datasetserviceSpy;

            beforeEach(function () {
                // return promise mock
                var d = $.Deferred();
                d.resolve([]); // make data service return success           

                // data service mock
                datasetserviceSpy = jasmine.createSpyObj("datasetserviceSpy", ["getItems"]);
                datasetserviceSpy.getItems.and.returnValue(d);
            });

            afterEach(function () {
                datasetserviceSpy = null;
            });

            it("Error, searchKeyword, and clients should be empty by default.", function () {
                // Arrange
                // Act
                var self = createViewModelForTesting();

                // Assert
                expect(self.error().length).toBe(0);
                expect(self.searchKeyword().length).toBe(0);
                expect(self.clients().length).toBe(0);
            });

            it("showError and clientsAvailable should be false by default.", function () {
                // Arrange
                // Act
                var self = createViewModelForTesting();

                // Assert
                expect(self.showError()).toBe(false);
                expect(self.clientsAvailable()).toBe(false);
            });

            it("addLink and editLink should be assigned param values.", function () {
                // Arrange
                var params = {
                    addLink: "add",
                    editLink: "edit"
                };

                // Act
                var self = createViewModelForTesting(params);

                // Assert
                expect(self.addLink).toBe("add");
                expect(self.editLink).toBe("edit");
            });

            it("showError should be true when error is not empty.", function () {
                // Arrange
                var self = createViewModelForTesting();

                // Act
                self.error("1");

                // Assert
                expect(self.showError()).toBe(true);
            });

            it("clientsAvailable should be true when filteredClients is not empty.", function () {
                // Arrange
                // Act
                var self = createViewModelForTesting();
                spyOn(self, "filteredClients").and.returnValue([{}]);

                // Assert
                expect(self.clientsAvailable()).toBe(true);
            });

            it("clientsUnavailable should be true or false depending on showError and clientsAvailable.", function () {
                clientsUnavailableTestCases.forEach(function (test) {
                    // Arrange
                    // Act
                    var self = createViewModelForTesting();
                    spyOn(self, "showError").and.returnValue(test.showError);
                    spyOn(self, "clientsAvailable").and.returnValue(test.clientsAvailable);

                    // Assert
                    expect(self.clientsUnavailable()).toBe(test.expected);
                });
            });

            it("filteredClients should return array of clients depending on searchKeyword.", function () {
                filteredClientsTestCases.forEach(function (test) {
                    // Arrange
                    // Act
                    var self = createViewModelForTesting();
                    spyOn(self, "searchKeyword").and.returnValue(test.keyword);
                    spyOn(self, "clients").and.returnValue(test.clients);

                    // Assert
                    expect(self.filteredClients()).toEqual(test.expected);
                });
            });

            it("load should call dataService.getItems() when view model created.", function () {
                // Arrange
                // Act
                var self = createViewModelForTesting();

                // Assert
                expect(self.dataService.getItems).toHaveBeenCalled();
            });

            it("load should set error with responseText when promise is rejected.", function () {
                // Arrange
                var d = $.Deferred();
                d.reject({ responseText: "test" });
                datasetserviceSpy.getItems.and.returnValue(d);

                // Act
                var self = createViewModelForTesting();

                // Assert
                expect(self.error().length).toBeGreaterThan(0);
                expect(self.error().indexOf("test")).toBeGreaterThan(0);
            });

            it("load should set clients with response and edit links when promise is resolved.", function () {
                // Arrange
                var params = { editLink: "" };
                var clients = [{ name: "test", editLink: "" }, { name: "test2", editLink: "" }];
                var d = $.Deferred();
                d.resolve(clients);
                datasetserviceSpy.getItems.and.returnValue(d);

                // Act
                var self = createViewModelForTesting(params);

                // Assert
                expect(self.clients()).toEqual(clients);
            });

            it("includeEditLinks should update provided array depending edit link test cases.", function () {
                includeEditLinksTestCases.forEach(function(test) {
                    // Arrange
                    var params = { editLink: test.editLink };
                    var self = createViewModelForTesting(params);

                    // Act
                    self.includeEditLinks(test.clients);

                    // Assert
                    expect(test.clients).toEqual(test.expected);    
                });
            });

            // private helper
            function createViewModelForTesting(paramsSpy) {
                var params = paramsSpy || {};

                var self = new clientsListViewModel(params, datasetserviceSpy);

                return self;
            }
        });
    }
);
