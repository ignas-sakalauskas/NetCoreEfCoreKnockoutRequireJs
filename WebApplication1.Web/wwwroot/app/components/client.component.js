define(["knockout"], function (ko) {
    ko.components.register("client", {
        viewModel: { require: basePath + "app/components/client.viewmodel.js" },
        template: { require: "text!" + basePath + "app/components/client.component.html" }
    });
});