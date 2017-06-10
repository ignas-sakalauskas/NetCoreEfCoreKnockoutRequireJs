define(["services/http.service", "services/log.service"], function ($http, $log) {
    "use strict";

    function categoriesDataService() {

        return {
            getCategories: getCategories
        };

        function getCategories() {
            return $http.get(basePath + "api/categories")
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

    return categoriesDataService();
});