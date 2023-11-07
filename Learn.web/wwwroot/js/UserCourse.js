$(".Login").click(function () {
    $.confirm({
        title: 'عدم دسترسی',
        content: 'برای دسترسی باید وارد حساب کاربری خود شوید. ',
        rtl: true,
        closeIcon: true,
        type: 'red',
        columnClass: 'col-md-5 col-md-offset-4',
        icon: 'zmdi zmdi-alert-octagon',
        buttons: {
            confirm: {
                text: 'ورود به سایت ',
                btnClass: 'btn-blue',
                action: function () {
                    var url = $("#url").prop("href")

                    window.location = url
                }
            },
            cancel: {
                text: 'ثبت نام',
                btnClass: 'btn-success',
                action: function () {
                    window.location = "/Register";
                }
            }
        }
    });
})
$(".factor").click(function () {
    $.confirm({
        title: 'عدم دسترسی',
        content: ' کاربر محترم قبل از دانلود باید این قسمت را خریداری نمایید.',
        rtl: true,
        closeIcon: true,
        type: 'red',
        columnClass: 'col-md-6 col-md-offset-4',
        icon: 'zmdi zmdi-alert-octagon',
        buttons: {
            confirm: {
                text: 'خرید ',
                btnClass: ' btn-info btn-lg',
                action: function () {
                    var url = $("#buy").prop("href")

                    window.location = url
                }
            },
            cancel: {
                text: 'لغو',
                btnClass: 'btn btn-default',
                action: function () {

                }
            }
        }
    });
})

$(function () {
    $(".dwn").each(function (index, element) {
        var href = $(this).attr("href");
        $(this).attr("hiddenhref", href);
        $(this).removeAttr("href");
    });
    $(".dwn").click(function () {
        url = $(this).attr("hiddenhref");
        // window.open(url);
        window.location.href = url;
    })
});
