$(document).ready(function () {
    //$('#example').DataTable();
    var t = $('#example').DataTable({
        "columnDefs": [{
            "searchable": false,
            "orderable": false,
            "targets": 0
        }],
        "order": [[1, 'asc']]
    });

    t.on('order.dt search.dt', function () {
        t.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
            cell.innerHTML = i + 1;
        });
    }).draw();
}); 

    $(document).ready(function () {
        $("#removesoret").removeClass("sorting");
        $("#removesoret").addClass("sorting_disabled");
        $("th").click(function () { $("#removesoret").removeClass("sorting") });
        $("#removesoret").click(function () { $("#removesoret").removeClass("sorting_desc") });
        $("#removesoret").click(function () { $("#removesoret").removeClass("sorting_asc") });

    })
