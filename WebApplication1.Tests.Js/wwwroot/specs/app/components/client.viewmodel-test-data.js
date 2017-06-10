var paramsTestCase = {
    clientId: 1,
    listLink: "list",
    mode: "mode",
    clientStatusEnum: { Item: 1 }
};

var pageTitleTestCases = [
    {
        add: true,
        edit: false,
        name: "",
        expected: "Add New Client"
    },
    {
        add: true,
        edit: false,
        name: "AAA",
        expected: "New Client: AAA"
    },
    {
        add: false,
        edit: true,
        name: "AAA",
        expected: "Client: AAA"
    },
    {
        add: false,
        edit: false,
        name: "AAA",
        expected: ""
    },
    {
        add: false,
        edit: false,
        name: "",
        expected: ""
    }
];

var editLabelTestCases = [
    {
        enabled: true,
        expected: "View"
    },
    {
        enabled: false,
        expected: "Edit"
    }
];

var canSaveTestCases = [
    {
        enabled: true,
        valid: true,
        expected: true
    },
    {
        enabled: true,
        valid: false,
        expected: false
    },
    {
        enabled: false,
        valid: false,
        expected: false
    },
    {
        enabled: false,
        valid: true,
        expected: false
    }
];

var modelTestObject = {
    clientId: 1,
    name: "name",
    email: "email",
    phone: "phone",
    fax: "fax",
    address: "address",
    status: "status",
    categoryId: 2
};

var setTestModelValues = function (self) {
    self.clientId(1);
    self.name("name");
    self.email("email");
    self.phone("phone");
    self.fax("fax");
    self.address("address");
    self.status("status");
    self.categoryId(2);
}