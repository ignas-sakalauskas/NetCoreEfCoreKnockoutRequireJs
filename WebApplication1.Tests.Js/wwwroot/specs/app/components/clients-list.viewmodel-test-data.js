var clientsUnavailableTestCases = [
    {
        showError: false,
        clientsAvailable: false,
        expected: true
    },
    {
        showError: true,
        clientsAvailable: false,
        expected: false
    },
    {
        showError: false,
        clientsAvailable: true,
        expected: false
    },
    {
        showError: true,
        clientsAvailable: true,
        expected: false
    }
];

var filteredClientsTestCases = [
    {
        clients: [],
        keyword: "",
        expected: []
    },
    {
        clients: [],
        keyword: "test",
        expected: []
    },
    {
        clients: [{ name: "a" }, { name: "b" }, { name: "c" }],
        keyword: "test",
        expected: []
    },
    {
        clients: [{ name: "a" }, { name: "b" }, { name: "c" }],
        keyword: "",
        expected: [{ name: "a" }, { name: "b" }, { name: "c" }]
    },
    {
        clients: [{ name: "a" }, { name: "b" }, { name: "c" }],
        keyword: "a",
        expected: [{ name: "a" }]
    },
    {
        clients: [{ name: "a" }, { name: "b" }, { name: "a" }],
        keyword: "a",
        expected: [{ name: "a" }, { name: "a" }]
    },
    {
        clients: [{ name: "A" }, { name: "b" }, { name: "a" }],
        keyword: "a",
        expected: [{ name: "A" }, { name: "a" }]
    },
    {
        clients: [{ name: "A" }, { name: "tesT" }, { name: "ccc" }],
        keyword: "TeST",
        expected: [{ name: "tesT" }]
    }
];

var includeEditLinksTestCases = [
    {
        editLink: "",
        clients: [],
        expected: []
    },
    {
        editLink: "",
        clients: [{ clientId: 1, name: "test" }],
        expected: [{ clientId: 1, name: "test", editLink: "" }]
    },
    {
        editLink: "/edit/CLIENTID",
        clients: [{ clientId: 1, name: "test" }, { clientId: 2, name: "test2" }],
        expected: [{ clientId: 1, name: "test", editLink: "/edit/1" }, { clientId: 2, name: "test2", editLink: "/edit/2" }]
    }
];