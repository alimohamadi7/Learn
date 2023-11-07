function RemoveOrder(id, orderid) {
    $("#factor_" + id).hide();
    $.ajax({
        url: "/UserPanel/MyOrders/RemoveOrder",
        method: "get",
        data: { id: id, orderid: orderid },
    }).done(function (result) {
        if (result == 0) {
            window.location = "/UserPanel/MyOrders";
        }
        $("#update").html(result);
    });
}
