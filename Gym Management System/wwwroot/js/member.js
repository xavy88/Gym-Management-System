﻿var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {

    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/admin/member/GetAll",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "name", "width": "30%" },
            { "data": "phone", "width": "20%" },
            { "data": "email", "width": "20%" },
            { "data": "trainer.name", "width": "20%" },
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="text-center">
                                <a href="/Admin/member/Upsert/${data}" class='btn btn-success text-white' style='cursor:pointer; width:50px;'>
                                    <i class='far fa-edit'></i>
                                </a>
                                &nbsp;
                                <a onclick=Delete("/Admin/member/Delete/${data}") class='btn btn-danger text-white' style='cursor:pointer; width:50px;'>
                                    <i class='far fa-trash-alt'></i>
                                </a>
                            </div>
                            `;
                }, "width": "40%"
            }
        ],
        "language": {
            "emptyTable": "No records found."
        },
        "width": "100%"
    });
}


function Delete(url) {
    swal({
        title: "Are you sure you want to delete?",
        text: "You will not be able to restore the content!",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Yes, delete it!",
        closeOnconfirm: true
    }, function () {
        $.ajax({
            type: 'DELETE',
            url: url,
            success: function (data) {
                if (data.success) {
                    toastr.success(data.message);
                    dataTable.ajax.reload();
                }
                else {
                    toastr.error(data.message);
                }
            }
        });
    });
}

