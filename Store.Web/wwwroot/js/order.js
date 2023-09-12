var dataTable;
var connectionOrder = new signalR.HubConnectionBuilder().withUrl("/hubs/order").build();



$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/orderHeader/getData' },
        "columns": [
            { data: 'id', "width": "5%" },
            { data: 'name', "width": "15%" },
            { data: 'phoneNumber', "width": "10%" },
            { data: 'user.email', "width": "15%" },
            { data: 'user.state', "width": "10%" },
            { data: 'orderTotal', "width": "10%" },
            {
                data: 'id',
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">                   
                    </div>`
                },
                "width": "25%"
            }
        ]
    });
}

connectionOrder.on("newOrder", () => {
    dataTable.ajax.reload();
    toastr.success("New order recived");
});

function fulfilled() {
    //do something on start
}
function rejected() {
    //rejected logs
}

connectionOrder.start().then(fulfilled, rejected);
