define(["jquery"], function (jquery) {

    function HttpService() {
        return {
            get: getMethod,
            post: postMethod,
            put: putMethod,
            delete: deleteMethod
        };
    }

    function getMethod(url) {
        return ajaxCall(url, "GET");
    };

    function postMethod(url, data) {
        return ajaxCall(url, "POST", data);
    };

    function putMethod(url, data) {
        return ajaxCall(url, "PUT", data);
    };

    function deleteMethod(url) {
        return ajaxCall(url, "DELETE");
    };

    function ajaxCall(url, method, data) {
        return jquery.ajax({
            url: url,
            method: method,
            data: JSON.stringify(data),
            headers: {
                'Accept': "application/json",
                'Content-Type': "application/json; charset=utf-8"
            },
            dataType: "json",
            jsonp: false
        });
    }

    return HttpService();
});
