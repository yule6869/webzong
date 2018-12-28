$(document).ready(function () {

    $("#yy span").each(function () {
        $(this).text($(this).text().substr(0, 10));
    });
    $("#zxxw span").each(function () {
        $(this).text($(this).text().substr(0, 10));
    });

    var wid = $(window).width();
    if (wid >= 1200) {


        var zhi3 = (wid - 1200) / 2;
        $(".breadcrumb").css("padding-left", zhi3);

        $("#kcbf").css({ "padding-left": zhi3, "padding-right": zhi3 });



        $(".touxia").css({ "padding-left": zhi3, "padding-right": zhi3 });

    }
    else {


    }



    $("#leibie div span").click(function () {
        $("#leibie div span").css({ "background": "", "color": "" });
        $(this).css({ "background": "#ff6a00", "color": "white" });
        var canshu = $(this).text();
        if (canshu == "全部") {
            $("#yy").children().show();
        }
        else {
            $("#yy").children().each(function () {
                if ($(this).data("leixing") == canshu) {
                    $(this).show();
                }
                else {
                    $(this).hide();
                }
            });

        }
    })
})
$(window).resize(function () {
    var wid = $(this).width();
    if (wid >= 1200) {


        var zhi3 = (wid - 1200) / 2;
        $(".breadcrumb").css("padding-left", zhi3);
        $(".touxia").css({ "padding-left": zhi3, "padding-right": zhi3 });

        $("#kcbf").css({ "padding-left": zhi3, "padding-right": zhi3 });




    }
    else {


        var zhi2 = 32.8 * (wid / 1920) + "px " + "0px " + "0px " + "0px";


        $(".breadcrumb").css("padding-left", "");

        $("#kcbf").css({ "padding-left": "", "padding-right": "" });



        $(".touxia").css({ "padding-left": "", "padding-right": "" });

    }
})

/**
 * * 时间戳转换日期
 * * @param <int> unixTime    待时间戳(秒)  
 * * @param <bool> isFull    返回完整时间(Y-m-d 或者 Y-m-d H:i:s) 
 * * @param <int>  timeZone   时区
 * */
function UnixToDate(unixTime, isFull, timeZone) {
    if (typeof (timeZone) == 'number') {
        unixTime = parseInt(unixTime) + parseInt(timeZone) * 60 * 60;
    }
    var time = new Date(unixTime * 1000);
    var ymdhis = "";
    ymdhis += time.getUTCFullYear() + "-";
    ymdhis += (time.getUTCMonth() + 1) + "-";
    ymdhis += time.getUTCDate();
    if (isFull === true) {
        ymdhis += " " + time.getUTCHours() + ":";
        ymdhis += time.getUTCMinutes() + ":";
        ymdhis += time.getUTCSeconds();
    }
    return ymdhis;
}