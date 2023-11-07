
var CommentCount = 0;
$("#submitcomment").click(function (e) {
    CommentCount++;
    var textarea = $("#comment_Comment").val();
    if (textarea.length == "") {
        e.preventDefault();
        $("#comment_Comment").addClass("text_area");
        $("#validation").html("لطفا متن نظر را وارد کنید.")
    }
    else {
        $("#comment_Comment").removeClass("text_area");
        $("#validation").html("")
    }

})

function GetDataload() {
   $("#listComment").empty();
    $("#comment_Comment").val("");
    var jsonData = {"courseId": CourseId};
$.ajax({
    type: 'get',
    url: '/Courses/ShowCommentInCreate',
    data: jsonData,
    contentType: "application/json",
    success: function (data) {
        
        if (data != null) {
            $.each(data, function (index, value) {
                if (value.parenId == null) {
                    
                    var item = "<div class='comment-row'id='repaly" + value.commentId + "' ><img src='/UserAvatar/" + value.userAvatar + "'/><div class='left-col'  ><h3> " + value.userName + " </h3> <span>" + value.createDate + "</span> <p>" + value.comment + "</p><button type='button' onclick='ReplayComent(" + value.commentId + ")' value='' class='btn reply-button zmdi zmdi-replay' >پاسخ</button><div class='col-md - 12 col - sm - 12 col - xs - 12' style='margin-top: 5px;><div class='form-group'><input type='hidden' name='CommentId' id='CommentId_" + value.commentId + "' value='" + value.commentId + "' /> <textarea style=' display:none'  class='form-control' id='comment_Comment_" + value.commentId + "' name='Comment' maxlength='800' placeholder='متن نظر'></textarea><span id='validation_" + value.commentId + "'></span> <div/> </div> </div><div class='col-xs - 12'> <button type='submit' id='submitcomment_" + value.commentId + "'onclick='SubmitComment(" + value.commentId + ")' style=' display:none' class='btn btn-success'> ثبت دیدگاه </button></div>"
                }
                if (value.parentId != null) {
                    var item = "<div class='comment-row' style=' border:none'><img src='/UserAvatar/" + value.userAvatar + "'/><div class='left-col'><h3> " + value.userName + " </h3> <span>" + value.createDate + "</span> <p>" + value.comment + "</p>  </div>   </div>"
                    $("#repaly" + value.parentId).append(item);
                   
                    return
                    
                }
                
                $("#listComment").append(item);
            })
            
        }

    },
    beforeSend: function () {
       

    },
    complete: function () {
        CommentCount
        if (CommentCount >3) {
            $("#btn-gedata").show();
        }
        
        $.notify("دیدگاه شما با موفقیت ثبت گردید و بعد از تایید نمایش داده میشود", "success", { position: "top center" });

    },
    error: function () {
        $.notify("متاسفانه مشکلی پیش آمده", "error");
        
    }

});
}

function GetData() {
  
    var jsonData = { "courseId":CourseId, "pageindex": pageIndex, "pagesize": pageSize
};
$.ajax({
    type: 'get',
    url: '/Courses/ShowComment',
    data: jsonData,
    contentType: "application/json",
    success: function (data) {
        if (data != null) {
            $.each(data, function (index, value) {
                if (value.parenId == null) { 
                    if (value.display == false) {
                        var item = "<div class='comment-row'id='repaly" + value.commentId + "' ><img src='/UserAvatar/" + value.userAvatar + "'/><div class='left-col'  ><h3> " + value.userName + " </h3> <span>" + value.createDate + "</span> <p>" + value.comment + "</p><div class='col-md - 12 col - sm - 12 col - xs - 12' style='margin-top: 5px;><div class='form-group'><input type='hidden' name='CommentId' id='CommentId_" + value.commentId + "' value='" + value.commentId + "' /> <textarea style=' display:none'  class='form-control' id='comment_Comment_" + value.commentId + "' name='Comment' maxlength='800' placeholder='متن نظر'></textarea><span id='validation_" + value.commentId + "'></span> <div/> </div> </div><div class='col-xs - 12'> <button type='submit' id='submitcomment_" + value.commentId + "'onclick='SubmitComment(" + value.commentId + ")' style=' display:none' class='btn btn-success'> ثبت دیدگاه </button></div>"

                    }
                    else {
                        var item = "<div class='comment-row'id='repaly" + value.commentId + "' ><img src='/UserAvatar/" + value.userAvatar + "'/><div class='left-col'  ><h3> " + value.userName + " </h3> <span>" + value.createDate + "</span> <p>" + value.comment + "</p><button type='button'id='Replaycom_" + value.commentId + "'  onclick='ReplayComent(" + value.commentId + ")' value='' class='btn reply-button zmdi zmdi-replay ' ><p class='rep'>پاسخ</p></button><div class='col-md - 12 col - sm - 12 col - xs - 12' style='margin-top: 5px;><div class='form-group'><input type='hidden' name='CommentId' id='CommentId_" + value.commentId + "' value='" + value.commentId + "' /> <textarea style=' display:none'  class='form-control' id='comment_Comment_" + value.commentId + "' name='Comment' maxlength='800' placeholder='متن نظر'></textarea><span id='validation_" + value.commentId + "'></span> <div/> </div> </div><div class='col-xs - 12'> <button type='submit' id='submitcomment_" + value.commentId + "'onclick='SubmitComment(" + value.commentId + ")' style=' display:none' class='btn btn-success'> ثبت دیدگاه </button></div>"

                    }
                }
                if (value.parentId != null) {
                    var item = "<div class='comment-row' style=' border:none'><img src='/UserAvatar/" + value.userAvatar + "'/><div class='left-col'><h3> " + value.userName + " </h3> <span>" + value.createDate + "</span> <p>" + value.comment + "</p>  </div>   </div>"
                    $("#repaly"+value.parentId).append(item);
                    return 
                   
                }
                $("#listComment").append(item);
            })
            
            if (pageSize>data.length) {
               $("#btn-gedata").hide()
            }
            pageIndex++;
        }

    },
    beforeSend: function () {
        $("#progress").show();

    },
    complete: function () {
        $("#progress").hide();


    },
    error: function () {
        $.notify("متاسفانه مشکلی پیش آمده", "error");
    }

});
            }


function ReplayComent(id) {
    
    $("#comment_Comment_" + id).toggle();
    $("#submitcomment_" + id).toggle();
    $("#comment_Comment_" + id).val("");
}
function SubmitComment(id) {
    var data = $("#comment_Comment_" + id).val()
    var CommentId = $("#CommentId_" + id).prop("value");
    var courseId = CourseId;
    var __RequestVerificationToken = $("input[name~='__RequestVerificationToken']").val();
    
    if (data.length == "") {
        $("#validation_" + id).css({ "color": "red" });
        $("#comment_Comment_" + id).addClass("text_area")
        $("#validation_" + id).html("متن نظر نباید خالی باشد");
    }
    else {
       
        $("#comment_Comment_" + id).removeClass("text_area")
        $("#validation_" + id).html("");
        var jsonData = {
            "Comment": data, "CourseId": courseId, "parentId": CommentId,"__RequestVerificationToken": __RequestVerificationToken}
        $.ajax({
            type: 'POST',
            url: '/Courses/CreateComment',
            data: jsonData,
            success: function () {

            },
            beforeSend: function () {
                

            },
            complete: function () {
               


            },
            error: function () {
                $.notify("متاسفانه مشکلی پیش آمده", "error");
            }

        }).done(function () {
            GetDataload();
            ReplayComent(id)
            $("#progress").hide();
        });

         
    }
}      

