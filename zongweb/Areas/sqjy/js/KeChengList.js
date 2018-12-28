$(document).ready(function () {
    var wid = $(window).width();
    wid = wid;
    if (wid >= 1200) {


        var zhi3 = (wid - 1200) / 2;
        $(".breadcrumb").css("padding-left", zhi3);
        $("#kcbf").css({ "padding-left": zhi3, "padding-right": zhi3 });
        $(".touxia").css({ "padding-left": zhi3, "padding-right": zhi3 });

    }
    else {


    }

    $("#tiao li").mouseover(function () {
        var h = $(".head").height();
        var wid = $(window).width();
        if (wid > 1000) {
            var j = 32.8 * (wid / 1920);
            h = h - j;
            $(this).css({ "height": h, "border-bottom": "4px solid #cc3333" });
        }

        $(this).addClass("daohangZ");
    })

    $("#tiao li").mouseleave(function () {
        $(this).removeAttr("style");

    })

    var leibie = $("#leibiee").text().trim();
    for (var i = 0; i < $("#leibie div span").length; i++) {
        if ($("#leibie div span").eq(i).text().trim() == leibie) {
            $("#leibie div span").eq(i).css({ "background": "#ff6a00", "color": "white" });
            break;
        }
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
    var wid = $(window).width();
    wid = wid;
    if (wid >= 1200) {
        var zhi = 30 * (wid / 1920) + "px" + " 0px " + 30 * (wid / 1920) + "px " + (wid - 1200) / 2 + "px";

        $(".touxia").css("padding", zhi);





        var zhi3 = (wid - 1200) / 2;
        $(".breadcrumb").css("padding-left", zhi3);

        $("#kcbf").css({ "padding-left": zhi3, "padding-right": zhi3 });





        $(".touxia").css({ "padding-left": zhi3, "padding-right": zhi3 });
    }
    else {
        var zhi = 30 * (wid / 1920) + "px" + " 0px " + 30 * (wid / 1920) + "px " + "0px";

        $(".touxia").css({ "padding-left": "", "padding-right": "" });
        var zhi2 = 32.8 * (wid / 1920) + "px " + "0px " + "0px " + "0px";


        $(".breadcrumb").css("padding-left", "");

        $("#kcbf").css({ "padding-left": "", "padding-right": "" });



        if (wid < 798) {
            $("#kcbf2").css({ "padding-left": "0px", "padding-right": "0px" });
            $("#shiping").css({ "padding-left": "0px", "padding-right": "0px" });
        }
        else {
            $("#kcbf2").css({ "padding-left": "", "padding-right": "" });
            $("#shiping").css({ "padding-left": "", "padding-right": "" });
        }
    }


});