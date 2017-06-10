define(["knockout"], function (ko) {
    ko.components.register("clients-list", {
        viewModel: { require: basePath + "app/components/clients-list.viewmodel.js" },
        template: { require: "text!" + basePath + "app/components/clients-list.component.html" }
    });
});