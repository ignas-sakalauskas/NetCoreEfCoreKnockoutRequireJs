define(["jquery", "services/http.service", "./http.service-test-data"], function (jquery, httpService) {

    describe("HTTP Service", function () {

        beforeEach(function () {
            var d = jquery.Deferred();
            d.resolve(testValue);
            spyOn(jquery, 'ajax').and.returnValue(d.promise());
        });

        it("Get calls ajax with correct params", function () {
            // Act
            httpService.get(testUrl);

            // Assert
            expect(jquery.ajax).toHaveBeenCalledWith(expectedGet);
        });

        it("Get returns jQuery promise which resolved to the expected value", function () {
            // Arrange
            var result;
            
            // Act
            httpService.get(testUrl).done(function(val) {
                result = val;
            });

            // Assert
            expect(result).toBe(testValue);
        });

        it("Post calls ajax with correct params", function () {
            // Act
            httpService.post(testUrl, testData);

            // Assert
            expect(jquery.ajax).toHaveBeenCalledWith(expectedPost);
        });

        it("Post returns jQuery promise which resolved to the expected value", function () {
            // Arrange
            var result;

            // Act
            httpService.post(testUrl, testData).done(function (val) {
                result = val;
            });

            // Assert
            expect(result).toBe(testValue);
        });

        it("Put calls jQuery.ajax with correct params", function () {
            // Act
            httpService.put(testUrl, testData);

            // Assert
            expect(jquery.ajax).toHaveBeenCalledWith(expectedPut);
        });

        it("Put returns jQuery promise which resolved to the expected value", function () {
            // Arrange
            var result;

            // Act
            httpService.put(testUrl, testData).done(function (val) {
                result = val;
            });

            // Assert
            expect(result).toBe(testValue);
        });

        it("Delete calls ajax with correct params", function () {
            // Act
            httpService.delete(testUrl);

            // Assert
            expect(jquery.ajax).toHaveBeenCalledWith(expectedDelete);
        });

        it("Delete returns jQuery promise which resolved to the expected value", function () {
            // Arrange
            var result;

            // Act
            httpService.delete(testUrl).done(function (val) {
                result = val;
            });

            // Assert
            expect(result).toBe(testValue);
        });
    });
});
