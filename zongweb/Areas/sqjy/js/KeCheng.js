var videoObject = {
    //playerID:'ckplayer01',//播放器ID，第一个字符不能是数字，用来在使用多个播放器时监听到的函数将在所有参数最后添加一个参数用来获取播放器的内容
    container: '#videoo', //容器的ID或className
    variable: 'player', //播放函数名称
    //loaded: 'loadedHandler', //当播放器加载后执行的函数
    loop: true, //播放结束是否循环播放
    //autoplay: true, //是否自动播放
    //duration: 500, //设置视频总时间
    // cktrack: 'material/srt.srt', //字幕文件
    //poster: 'material/poster.jpg', //封面图片
    // preview: { //预览图片
    //     file: ['material/mydream_en1800_1010_01.png', 'material/mydream_en1800_1010_02.png'],
    //     scale: 2
    // },
    config: '', //指定配置函数
    debug: true, //是否开启调试模式
    //flashplayer: true, //强制使用flashplayer
    drag: 'start', //拖动的属性
    seek: 0, //默认跳转的时间
    //playbackrate:1,//默认速度的编号，只对html5有效,设置成-1则不显示倍速
    //advertisements:'website:ad.json',
    //front:'frontFun',//上一集的操作函数
    //next:'nextFun',//下一集的操作函数
    //广告部分开始

    //广告部分结束
    promptSpot: [ //提示点
        {
            words: '提示点文字01',
            time: 30
        },
        {
            words: '提示点文字02',
            time: 150
        }
    ],
    //mobileCkControls:true,//是否在移动端（包括ios）环境中显示控制栏
    //live:true,//是否是直播视频，true=直播，false=点播
    //video: [
    //    [videodizhi, 'video/mp4', '中文标清', 0]

    //]
};

var player;
$(document).ready(function () {


    var wid = $(window).width();
    wid = wid;
    if (wid >= 1200) {


        var zhi3 = (wid - 1200) / 2;
        $(".breadcrumb").css("padding-left", zhi3);

        $("#kcbf").css({ "padding-left": zhi3, "padding-right": zhi3 });

    }
    else {


    }
    if (wid < 798) {
        $("#kcbf2").css({ "padding-left": "0px", "padding-right": "0px" });
        $("#shiping").css({ "padding-left": "0px", "padding-right": "0px" });
    }

    var videodizhi = $("#videodizhi").text().trim();
    if (videodizhi == "") {
        $("#login").modal();
    }
    else {
        videoObject.video = [[videodizhi, 'video/mp4', '中文标清', 0]];
        player = new ckplayer(videoObject);
    }
    $("#kclb").children().eq(0).addClass("kuangzhong");
    $("#kclb").children().click(function () {
        $("#kclb").children().removeClass("kuangzhong");
        $(this).addClass("kuangzhong");
        var videodizhi = $(this).data("dizhi");
        videoObject.video = [[videodizhi, 'video/mp4', '中文标清', 0]];
        player = new ckplayer(videoObject);
    });

})

$(window).resize(function () {
    var wid = $(window).width();
    wid = wid;
    if (wid >= 1200) {


        var zhi3 = (wid - 1200) / 2;
        $(".breadcrumb").css("padding-left", zhi3);

        $("#kcbf").css({ "padding-left": zhi3, "padding-right": zhi3 });


    }
    else {


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



