define(function () {
    "use strict";

    function log() {
        return {
            error: error
        }
    }

    function error(message) {
        if (window.console) window.console.log(message);
    };

    return log();

});