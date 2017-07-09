var testUrl = "https://ignas.me/";
var testHeaders = {
    'Accept': "application/json",
    'Content-Type': "application/json; charset=utf-8"
};

var testData = { name: "ignas" };
var testDataJson = JSON.stringify(testData);
var testValue = 1111;

var expectedGet = { url: testUrl, method: "GET", data: undefined, headers: testHeaders, dataType: "json", jsonp: false };
var expectedPost = { url: testUrl, method: "POST", data: testDataJson, headers: testHeaders, dataType: "json", jsonp: false };
var expectedPut = { url: testUrl, method: "PUT", data: testDataJson, headers: testHeaders, dataType: "json", jsonp: false };
var expectedDelete = { url: testUrl, method: "DELETE", data: undefined, headers: testHeaders, dataType: "json", jsonp: false };