define(["knockout", "services/clients-data.service"],
    function (ko, clientsDataService) {
        function clientsListViewModel(params, dataService) {
            var self = this;
            // Read params
            self.addLink = params.addLink;
            self.editLink = params.editLink;

            // use injected version for mocking in unit tests
            self.dataService = dataService || clientsDataService;

            // Assign default values
            self.clients = ko.observableArray([]);
            self.error = ko.observable("");
            self.searchKeyword = ko.observable("");

            // Computed values
            self.clientsAvailable = ko.pureComputed(function () {
                return self.filteredClients().length > 0;
            });

            self.clientsUnavailable = ko.pureComputed(function () {
                return !self.showError() && !self.clientsAvailable();
            });

            self.showError = ko.pureComputed(function () {
                return self.error().length > 0;
            });

            self.filteredClients = ko.pureComputed(function () {
                var filter = self.searchKeyword().toLowerCase();
                if (!filter) {
                    return self.clients();
                }

                return ko.utils.arrayFilter(self.clients(), function (client) {
                    return client.name.toLowerCase().indexOf(filter) > -1;
                });
            });

            self.includeEditLinks = function (array) {
                // Add edit link to each object
                ko.utils.arrayForEach(array, function (obj) {
                    obj.editLink = self.editLink.replace("CLIENTID", obj.clientId);
                });
            }

            self.load = function () {
                self.dataService.getItems().done(function (data) {
                    self.includeEditLinks(data);
                    self.clients(data);
                }).fail(function (response) {
                    self.error("Error retrieving clients: " + response.responseText);
                });
            }

            // Entry point
            self.load();
        }

        return clientsListViewModel;
    });