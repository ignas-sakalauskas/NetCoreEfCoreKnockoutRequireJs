define(["knockout", "services/clients-data.service", "services/formatting.service", "services/categories-data.service"],
    function (ko, clientsDataService, formattingService, categoriesDataService) {

        function clientViewModel(params, dataService, formatService, categoriesService) {
            var self = this;
            // Read params
            self.clientId = ko.observable(params.clientId || 0);
            self.listLink = params.listLink || "";
            self.mode = params.mode || "";
            self.clientStatusEnum = params.clientStatusEnum || {};

            // use injected version for mocking in unit tests
            self.dataService = dataService || clientsDataService;
            self.formatService = formatService || formattingService;
            self.categoriesService = categoriesService || categoriesDataService;

            // Local constants
            var maxLength = 100;
            var minLength = 3;

            // Assign default model values
            self.name = ko.observable("").extend({ required: true, minLength: minLength, maxLength: maxLength });
            self.email = ko.observable("").extend({ email: true });
            self.phone = ko.observable("").extend({ maxLength: maxLength });
            self.fax = ko.observable("").extend({ maxLength: maxLength });
            self.address = ko.observable("").extend({ maxLength: maxLength });
            self.status = ko.observable(0).extend({ required: true });
            self.createdOn = ko.observable("");
            self.categoryId = ko.observable(0).extend({ required: true });

            // Assign default values
            self.statuses = ko.observableArray([]);
            self.categories = ko.observableArray([]);
            // Convert enum to object array for dropdown
            Object.keys(self.clientStatusEnum).map(function (type) {
                self.statuses.push({ name: type, id: self.clientStatusEnum[type] });
            });
            self.error = ko.observable("");
            self.isAddMode = self.mode.toLowerCase() === "add";
            self.isEditMode = self.mode.toLowerCase() === "edit";
            self.editEnabled = ko.observable(false);

            // Computed variables
            self.pageTitle = ko.pureComputed(function () {
                if (self.isAddMode) {
                    if (self.name()) {
                        return "New Client: " + self.name();
                    }

                    return "Add New Client";
                }

                if (self.isEditMode) {
                    return "Client: " + self.name();
                }

                return "";
            });

            self.editLabel = ko.pureComputed(function () {
                if (self.editEnabled()) {
                    return "View";
                }

                return "Edit";
            });

            self.showError = ko.pureComputed(function () {
                return self.error().length > 0;
            });

            self.canSave = ko.pureComputed(function () {
                return self.editEnabled() && self.isValid();
            });

            // Global model validation
            self.isValid = ko.pureComputed(function () {
                var errors = ko.validation.group(self);
                return errors().length === 0;
            });

            // Events
            self.saveClick = function () {
                var obj = {
                    clientId: self.clientId(),
                    name: self.name(),
                    email: self.email(),
                    phone: self.phone(),
                    fax: self.fax(),
                    address: self.address(),
                    status: self.status(),
                    categoryId: self.categoryId()
                };

                if (self.isAddMode) {
                    self.createClient(obj);
                }

                if (self.isEditMode) {
                    self.updateClient(obj);
                }
            };

            self.deleteClick = function () {
                self.dataService.deleteItem(self.clientId())
                    .done(function () {
                        self.redirectToList();
                    }).fail(function (response) {
                        self.error("Error deleting client! " + response.responseText);
                    });
            };

            self.enableEdit = function () {
                self.editEnabled(!self.editEnabled()); // invert value
            };

            // Functions
            self.redirectToList = function () {
                window.location.href = self.listLink;
            };

            self.createClient = function (client) {
                self.dataService.createItem(client)
                    .done(function () {
                        self.redirectToList();
                    }).fail(function (response) {
                        self.error("Error saving client! " + response.responseText);
                    });
            }

            self.updateClient = function (client) {
                self.dataService.updateItem(self.clientId(), client)
                    .done(function () {
                        self.redirectToList();
                    }).fail(function (response) {
                        self.error("Error updating client! " + response.responseText);
                    });
            }

            self.loadClient = function(clientId) {
                self.dataService.getItem(clientId)
                    .done(function (response) {
                        self.name(response.name);
                        self.email(response.email);
                        self.phone(response.phone);
                        self.fax(response.fax);
                        self.address(response.address);
                        self.status(response.status);
                        self.createdOn(self.formatService.formatDate(response.createdOn));
                        self.categoryId(response.categoryId);
                    }).fail(function (response) {
                        self.error("Error getting client: " + response.responseText);
                    });
            }

            self.load = function () {
                // Always load categories
                self.categoriesService.getCategories()
                    .done(function (response) {
                        // Set categories
                        self.categories(response);
                        // Load existing client record if edit mode active
                        if (self.isEditMode) {
                            self.loadClient(self.clientId());
                        }
                    }).fail(function (response) {
                        self.error("Error retrieving categories: " + response.responseText);
                    });
            };

            // Set default values for Add mode
            if (self.isAddMode) {
                self.editEnabled(true);
            }

            // Entry point
            self.load();
        }

        return clientViewModel;
    });