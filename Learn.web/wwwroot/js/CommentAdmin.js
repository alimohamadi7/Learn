
function chenge(id, val) {
    $("#unimg_" + id).attr("src", "/images/001.png");
    $("#unimg_" + id).attr("Id", "reimg_" + id + "");
    $("#un_" + id).prop('checked', true);
    $("#un_" + id).attr("Id", "re_" + id + "");
    $("#untext_" + id).removeClass("text-red");
    $("#untext_" + id).addClass("text-success");
    $("#untext_" + id).html("خوانده شده");
    $("#untext_" + id).attr("Id", "retext_" + id + "");
    $("#re_" + id).attr("onchange", "chenge2(" + id + ")");
    $("#re_" + id).attr("attr", "");
    $("#re_" + id).attr("atr", "checked");
   // if (val != true) {

        //setTimeout(function () {
        //    var f = $('input:hidden[name="__RequestVerificationToken"]').val();
        //    var jsonData = { "CommentId": id, "__RequestVerificationToken": f };
        //    var attr = $("#re_" + id).attr("attr");
        //    if (attr == "") {
        //        $.ajax({
        //            type: 'POST',
        //            url: '/Admin/Courses/CourseComment/ReadComment',
        //            data: jsonData,
        //            success: function (data) {
        //                $("#tb_" + id).hide("slow");
        //                loaddata();

        //            },

        //            error: function () {

        //                alert("متاسفانه مشکلی پیش آمده");
        //            }

        //        });
        //    }
        //}, 3000)
   // }

}
function chenge2(id) {
    $("#reimg_" + id).attr("src", "/images/multi.png");
    $("#reimg_" + id).attr("Id", "unimg_" + id + "");
    $("#re_" + id).prop('checked', true);
    $("#re_" + id).attr("Id", "un_" + id + "");
    $("#retext_" + id).removeClass("text-success");
    $("#retext_" + id).addClass("text-red");
    $("#retext_" + id).html("خوانده نشده");
    $("#retext_" + id).attr("Id", "untext_" + id + "");
    $("#un_" + id).attr("onchange", "chenge(" + id + ")");
    $("#un_" + id).attr("attr", 2)
    $("#un_" + id).attr("atr", "");
}
function Edit(id, matn) {

    $("#myModal").modal();
    $("#myModalLabel").html("ویرایش دیدگاه کاربر");
    $("#myModalBody").empty();
    $("#myModalBody").append("<div class='comment-row'id='repaly' ><div class='col-md - 12 col - sm - 12 col - xs - 12' style='margin-top: 5px;><div class='form-group'><input type='hidden' name='CommentId' id='CommentId' value='" + id + "' /> <textarea   class='form-control' value='' id='comment_Comment' name='Comment' style='resize: none;height:177px' maxlength='800' placeholder='متن نظر'></textarea><span id='validation'></span> <div/> </div> </div><div class='col-xs - 12'> <button type='submit' id='submitcomment' onclick='EditComment(" + id + ")'  class='btn btn-warning btn-block' style='margin-top:7px'> ویرایش و تایید </button></div>")
    $("#comment_Comment").html(matn);
    return false;
}

function EditComment(id) {
    chenge(id, true);
    var data = $("#comment_Comment").val();
    var f = $('input:hidden[name="__RequestVerificationToken"]').val();
    var jsonData = { "Comment": data, "CommentId": id, "__RequestVerificationToken": f };
    $.ajax({
        type: 'POST',
        url: '/Admin/Courses/CourseComment/EditComment',
        data: jsonData,
        success: function (data) {
            $("#myModal").modal('hide')
            $("#comment_Comment_" + id).val(data)
            setTimeout(function () {
                $("#tb_" + id).hide("slow")
                loaddata();
            }, 3000);

        },
        beforeSend: function () {

        },
        complete: function () {



        },
        error: function () {

            alert("متاسفانه مشکلی پیش آمده");
        }

    })
}
function Replay(id, parentid) {
    $("#myModal").modal();
    $("#myModalLabel").html("پاسخ به دیدگاه کاربر");
    $("#myModalBody").empty();
    $("#myModalBody").append("<div class='comment-row'id='repaly' ><div class='col-md - 12 col - sm - 12 col - xs - 12' style='margin-top: 5px;><div class='form-group'><input type='hidden' name='CommentId' id='CommentId' value='" + id + "' /> <textarea   class='form-control' value='' id='comment_Comment' name='Comment' style='resize: none;height:177px' maxlength='800' placeholder='متن نظر'></textarea><span id='validation_'></span> <div/> </div> </div><div class='col-xs - 12'> <button type='submit' id='submitcomment' onclick='ReplayCommentAdmin(" + id + "," + parentid + ")'  class='btn btn-success btn-block' style='margin-top:7px'> پاسخ وتایید </button></div>")
    return false;
}
function ReplayCommentAdmin(id, parentid, CorseId) {
    if (parentid == null) {

        parentid = id;
    }

    var data = $("#comment_Comment").val();
    if (data == "") {
        $("#comment_Comment").addClass("text_area");
        $("#validation_").html(".متن نباید خالی باشد")
        return false;

    }
    else {
        $("#comment_Comment").removeClass("text_area");
        $("#validation_").html("");
    }
    var f = $('input:hidden[name="__RequestVerificationToken"]').val();
    var jsonData = { "Comment": data, "CourseId": CorseId, "parentId": parentid, "commentId": id, "__RequestVerificationToken": f }
    $.ajax({
        type: 'POST',
        url: '/Courses/CreateComment',
        data: jsonData,
        success: function () {
            $("#myModal").modal('hide');
            chenge(id, true);
            setTimeout(function () {
                $("#tb_" + id).hide("slow");
                loaddata();
            }, 3000);

        },
        beforeSend: function () {


        },
        complete: function () {



        },
        error: function () {
            alert("متاسفانه مشکلی پیش آمده");
        }

    }).done(function () {

    });
}
function loaddata() {

    $("#update").load("/Admin/Courses/CourseComment?checkajax=" + true);
};
function Delete(id) {

    $.alert({
        title: 'حذف پیام کاربر',
        content: 'آبا از حذف مطمئن هستید؟',
        type: 'red',
        rtl: true,
        closeIcon: true,
        buttons: {
            confirm: {
                text: 'تایید',
                btnClass: 'btn-red col-md-5',
                action: function () {
                    var f = $('input:hidden[name="__RequestVerificationToken"]').val();
                    var jsonData = { "CommentId": id, "__RequestVerificationToken": f }
                    $.ajax({
                        type: 'POST',
                        url: '/Admin/Courses/CourseComment/DeleteComment',
                        data: jsonData,
                        success: function () {
                            setTimeout(function () {
                                $("#tb_" + id).hide("slow");
                                loaddata();
                            }, 2000);

                        },
                        beforeSend: function () {


                        },
                        complete: function () {



                        },
                        error: function () {
                            alert("متاسفانه مشکلی پیش آمده");
                        }

                    }).done(function () {

                    });
                }
            },
            cancel: {
                text: 'انصراف',
                btnClass: 'col-md-5',
                action: function () {
                }
            }
        }
    });

    return false;

}

function submitcomment() {
    var favorite = [];
    $("input").each(function (index) {

        if ($(this).attr('type') == 'checkbox' && $(this).attr('atr') == 'checked') {
            favorite.push($(this).val());
        }
    });
    var f = $('input:hidden[name="__RequestVerificationToken"]').val();
    var jsonData = { "CommentId": favorite, "__RequestVerificationToken": f };

    if (favorite.length != 0) {
    $.ajax({
        type: 'POST',
        url: '/Admin/Courses/CourseComment/ReadComment',
        data: jsonData,
        success: function (data) {
            $.each(favorite, function (index, value) {
                $("#tb_" + value).hide("slow");
            });

            loaddata();

        },

        error: function () {

            alert("متاسفانه مشکلی پیش آمده");
        }

    });
    }
}