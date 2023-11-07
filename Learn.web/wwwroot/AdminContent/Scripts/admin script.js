// show picture before upload
function readURL(input) {

    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#imgAvatar').attr('src', e.target.result);
        }

        reader.readAsDataURL(input.files[0]);
    }
}
//for create user
$("#CreateUserViewModel_UserAvatar").change(function () {
    readURL(this);
   
});
//for edit user
$("#EditUserViewModel_UserAvatar").change(function () {
    readURL(this);
});

    //for img create course
    function readURL(input) {

        if (input.files && input.files[0]) {
            var reader = new FileReader();

            reader.onload = function (e) {
                $('#imgCourse').attr('src', e.target.result);
            }

            reader.readAsDataURL(input.files[0]);
        }
    }
    $("#imgCourseUp").change(function () {
        readURL(this);
    });
  
////for group and subgroup course
        $("#CourseViewModel_GroupId").change(function () {
            $("#CourseViewModel_SubGroup").empty();
            $.getJSON("/Home/GetSubGroups/" + $("#CourseViewModel_GroupId :selected").val(),
                function (data) {
                    $.each(data,
                        function () {
                            $("#CourseViewModel_SubGroup").append('<option value=' + this.value + '>' + this.text + '</option>');

                        });

                });

        });

//CKEDITOR
CKEDITOR.replace('CourseViewModel_CourseDescription', {
    customConfig: '/lib/ckeditor/Config.js'
});
//video priviwe
$(document).on("change", "#CourseViewModel_demoUp", function (evt) {
    var $source = $('#video_here');
    $source[0].src = URL.createObjectURL(this.files[0]);
    $source.parent()[0].load();
});

