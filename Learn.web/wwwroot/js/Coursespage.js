$("#available-filter-1").click(function () {
  
    $("#getType").prop("value", "all");
    $("#formFilter").submit()
    
    
})
$("#available-filter-2").click(function () {

    $("#getType").prop("value", "buy");
    $("#formFilter").submit()


})
$("#available-filter-3").click(function () {

    $("#getType").prop("value", "free");
    $("#formFilter").submit()


})
$("#selectsort").change(function () {
    var orderByType =$( this).val();
    $("#orderByType").prop("value", orderByType);
    $("#formFilter").submit()
})
function changeGroup() {
    $("#formFilter").submit();
  
}
$(document).ready(function () {
    //get values all checked input when load page
    var favorite = [];
    $.each($("input[name='selectedGroups']:checked"), function () {
        favorite.push($(this).val());
    });
    //create new link add "&$selectGroups=" value 
    $.each(favorite, function (index, value) {
        var backurl = $("#back").attr("href");
        var nexturl = $("#next").attr("href");
        backurl = backurl + "&selectedGroups=" + value;
        nexturl = nexturl + "&selectedGroups=" + value;
        $("#next").attr("href", nexturl);
        $("#back").attr("href", backurl);
        for (var i = 1; i <=count; i++) {
            var oldurl = $("#page_" + i).attr("href");
            oldurl = oldurl + "&selectedGroups=" + value;
            $("#page_" + i).attr("href", oldurl);
        }

    });
  
}) 
         
function filterbyprice() {
    var maxprice = $("#max-text").text();
    var minprice = $("#min-text").text();
    $("#startPrice").prop("value", minprice);
    $("#endPrice").prop("value", maxprice);
    $("#formFilter").submit();
}