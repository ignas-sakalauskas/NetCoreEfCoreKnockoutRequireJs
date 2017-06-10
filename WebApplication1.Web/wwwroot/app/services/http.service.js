define(["jquery"], function (jquery) {

    jquery.each(["post", "put", "delete"], function (i, method) {
        jquery[method] = function (url, data, callback) {

            return jquery.ajax({
                url: url,
                headers: {
                    'Accept': "application/json",
                    'Content-Type': "application/json; charset=utf-8"
                },
                type: method,
                dataType: "json",
                data: JSON.stringify(data),
                success: callback,
                jsonp: false
            });
        };
    });

    function HttpService() {
        return {
            get: get,
            post: post,
            put: put,
            delete: deleteMethod
        };
    }

    function get(url) {
        return jquery.get(url);
    };

    function post(url, data) {
        return jquery.post(url, data);
    };

    function put(url, data) {
        return jquery.put(url, data);
    };

    function deleteMethod(url) {
        return jquery.delete(url);
    };

    return HttpService();
});
