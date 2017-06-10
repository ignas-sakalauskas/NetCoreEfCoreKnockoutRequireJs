define(["services/http.service", "services/log.service"], function ($http, $log) {
    "use strict";

    function clientsDataService() {

        return {
            getItems: getItems,
            getItem: getItem,
            createItem: createItem,
            updateItem: updateItem,
            deleteItem: deleteItem
        };

        function getItems() {
            return $http.get(basePath + "api/clients")
                .done(callSuccess)
                .fail(callFailed);

            function callSuccess(response) {
                return response;
            }

            function callFailed(error) {
                $log.error(error.statusText);
            }
        }

        function getItem(id) {
            return $http.get(basePath + "api/clients/" + id)
                .done(callSuccess)
                .fail(callFailed);

            function callSuccess(response) {
                return response;
            }

            function callFailed(error) {
                $log.error(error.statusText);
            }
        }

        function createItem(newItem) {
            return $http.post(basePath + "api/clients", newItem)
                .done(callSuccess)
                .fail(callFailed);

            function callSuccess(response) {
                return response;
            }

            function callFailed(error) {
                $log.error(error.statusText);
            }
        }

        function updateItem(id, item) {
            return $http.put(basePath + "api/clients/" + id, item)
                .done(callSuccess)
                .fail(callFailed);

            function callSuccess(response) {
                return response;
            }

            function callFailed(error) {
                $log.error(error.statusText);
            }
        }

        function deleteItem(id) {
            return $http.delete(basePath + "api/clients/" + id)
                .done(callSuccess)
                .fail(callFailed);

            function callSuccess(response) {
                return response;
            }

            function callFailed(error) {
                $log.error(error.statusText);
            }
        }
    }

    return clientsDataService();
});