$(document).ready(function () {
    $("#jsdiv").hide();
    var wid = $(window).width();
    wid = wid + 16;
    if (wid >= 1200) {
        var zhi = 30 * (wid / 1920) + "px" + " 0px " + 30 * (wid / 1920) + "px " + (wid - 1200) / 2 + "px";
        $("#logo").css("padding", zhi);

        var zhi2 = 32.8 * (wid / 1920) + "px " + (wid - 1200) / 2 + "px " + "0px " + "0px";
        $("#lan").css("padding", zhi2);

        var zhi5 = (wid - 1200) / 2;
        $("#qqq1").css({ "padding-left": zhi5, "padding-right": zhi5 });
    }
    else
    {
        var zhi = 30 * (wid / 1920) + "px" + " 0px " + 30 * (wid / 1920) + "px " + "0px";
        $("#logo").css("padding", zhi);

        var zhi2 = 32.8 * (wid / 1920) + "px " + "0px " + "0px " + "0px";
        $("#lan").css("padding", zhi2);


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

    $(window).resize(function () {
        var wid = $(this).width();
        if (wid >= 1200) {
            var zhi = 30 * (wid / 1920) + "px" + " 0px " + 30 * (wid / 1920) + "px " + (wid - 1200) / 2 + "px";
            $("#logo").removeAttr("style").css("padding", zhi);

            var zhi2 = 32.8 * (wid / 1920) + "px " + (wid - 1200) / 2 + "px " + "0px " + "0px";
            $("#lan").css("padding", zhi2);

           

            var zhi5 = (wid - 1200) / 2;
            $("#qqq1").css({ "padding-left": zhi5, "padding-right": zhi5 });


        }
        else {
            var zhi = 30 * (wid / 1920) + "px" + " 0px " + 30 * (wid / 1920) + "px " + "0px";
            $("#logo").removeAttr("style").css("padding", zhi);

            var zhi2 = 32.8 * (wid / 1920) + "px " + "0px " + "0px " + "0px";
            $("#lan").css("padding", zhi2);

            $("#qqq1").css({ "padding-left": "", "padding-right": "" });

            

        }
    });
})