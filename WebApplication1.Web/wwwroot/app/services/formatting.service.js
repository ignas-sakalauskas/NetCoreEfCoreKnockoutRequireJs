define([], function () {
    "use strict";

    function formattingService() {

        return {
            formatDate: formatDate
        };

        function formatDate(dateString) {

            var monthNames = [
                "Jan", "Feb", "Mar",
                "Apr", "May", "Jun", "Jul",
                "Aug", "Sep", "Oct",
                "Nov", "Dec"
            ];

            var date = new Date(dateString);
            var day = date.getDate();
            var monthIndex = date.getMonth();
            var hours = date.getHours();;
            var minutes = date.getMinutes();
            var seconds = date.getSeconds();
            var year = date.getFullYear();

            var result = padDatePart(day) + " " + monthNames[monthIndex] + " " + year + " " + padDatePart(hours) + ":" + padDatePart(minutes) + ":" + padDatePart(seconds);

            return result;
        }

        // Zeros padding helper
        function padDatePart(val) {
            return ("0" + val).slice(-2);
        }
    }

    return formattingService();
});