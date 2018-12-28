$(document).ready(function () {
    $("#yy div").each(function () {
        if ($(this).data("leixing") == "社区新闻") {
            $(this).show();
        }
    });
    $("#yy div[data-leixing='社区新闻']:first").removeClass().addClass("item");
   

    $("#yy").children().click(function () {
        $(this).siblings().removeClass().addClass("item2");
        $(this).removeClass().addClass("item");

    });

    $("#yy").find("span").each(function () {

        var shijianjian = $(this).text().substr(0,10);
        $(this).text(shijianjian);
    });


    var wid = $(window).width();

    wid = wid + 16;
    if (wid >= 1200) {

        var zhi3 = "0 " + (wid - 1200) / 2 + "px 0 " + (wid - 1200) / 2 + "px";
        $(".guanyu").css("margin", zhi3);

        var zhi4 = "15px " + (wid - 1200) / 2 + "px 0 " + (wid - 1200) / 2 + "px";
        $(".xinwen").css("padding", zhi4);

        $(".kecheng").css("padding", zhi4);

    }
    else {


    }



    var widimg = $("#xinwentu").width();
    var heitimg = widimg * 0.7;
    $("#xinwentu img").css("height", heitimg);


    $(".zu div").click(function () {
        $(".zu div").css({ "color": "", "background": "" });
        $(this).css({ "color": "white", "background": "#d73240" });
        var leixing = $(this).text();

        $("#yy").children().each(function () {
            $(this).hide();
            if ($(this).data("leixing") == leixing) {
                $(this).show();
            }
        });
        $("#yy div[data-leixing='" + leixing + "']:first").removeClass().addClass("item");

    })

    $("#tu1").show();
    $("#xinwentu a").first().show();
    var dangqian = 0;
    $("#xinwentu span").click(function () {
        $("#xinwentu span").eq(2 - dangqian).removeClass();
        $("#xinwentu span").eq(2 - dangqian).addClass("dian");

        $(this).removeClass();
        $(this).addClass("zhong");

        var nub = $(this).attr("id");
        nub = nub.substr(4, 1);
        var zz = nub - 1;
        nub = "#tu" + nub;

        $("#xinwentu img").eq(dangqian).fadeOut(0, function () {
            $(nub).fadeIn(0).next().show();
        }).next().hide();
        dangqian = zz;


    })

    function xinwentuzhuan() {
        $("#xinwentu span").eq(2 - dangqian).removeClass();
        $("#xinwentu span").eq(2 - dangqian).addClass("dian");
        dangqian = dangqian + 1;
        if (dangqian > 2)
            dangqian = 0;
        $("#xinwentu span").eq(2 - dangqian).removeClass();
        $("#xinwentu span").eq(2 - dangqian).addClass("zhong");

        var nub = $("#xinwentu span").eq(2 - dangqian).attr("id");
        nub = nub.substr(4, 1);
        var zz = nub - 1;
        nub = "#tu" + nub;

        $("#xinwentu img").eq(dangqian - 1).fadeOut(0, function () {
            $(nub).fadeIn(0).next().show();
        }).next().hide();
    }

    setInterval(xinwentuzhuan, 15000);

})

$(window).resize(function () {


    var wid = $(this).width();
    if (wid >= 1200) {
        var zhi = 30 * (wid / 1920) + "px" + " 0px " + 30 * (wid / 1920) + "px " + (wid - 1200) / 2 + "px";
        $("#logo").removeAttr("style").css("padding", zhi);

        var zhi2 = 32.8 * (wid / 1920) + "px " + (wid - 1200) / 2 + "px " + "0px " + "0px";
        $("#lan").css("padding", zhi2);

        var zhi3 = "0 " + (wid - 1200) / 2 + "px 0 " + (wid - 1200) / 2 + "px";
        $(".guanyu").css("margin", zhi3);

        var zhi4 = "15px " + (wid - 1200) / 2 + "px 0 " + (wid - 1200) / 2 + "px";
        $(".xinwen").css("padding", zhi4);

        $(".kecheng").css("padding", zhi4);

        var zhi5 = (wid - 1200) / 2;
        $("#qqq1").css({ "padding-left": zhi5, "padding-right": zhi5 });

    }
    else {
        var zhi = 30 * (wid / 1920) + "px" + " 0px " + 30 * (wid / 1920) + "px " + "0px";
        $("#logo").removeAttr("style").css("padding", zhi);

        var zhi2 = 32.8 * (wid / 1920) + "px " + "0px " + "0px " + "0px";
        $("#lan").css("padding", zhi2);

        $(".guanyu").removeAttr("style");

        $(".xinwen").removeAttr("style");

        $(".kecheng").removeAttr("style");

        $("#qqq1").css({ "padding-left": "", "padding-right": "" });

    }
    widimg = $("#xinwentu").width();
    heitimg = widimg * 0.7;
    $("#xinwentu img").css("height", heitimg);

});
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