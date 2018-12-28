//var myChart = echarts.init(document.getElementById('main'));

// Dom7
var $$ = Dom7;

// Theme
var theme = 'auto';
if (document.location.search.indexOf('theme=') >= 0) {
    theme = document.location.search.split('theme=')[1].split('&')[0];
}
//dingshiqi
var timer = null;

// Init App
var app = new Framework7({
    id: 'io.framework7.testapp',
    root: '#app',
    theme: 'md',
    data: function () {
        return {
            user: {
                firstName: 'John',
                lastName: 'Doe',
            },
        };
    },
    methods: {
        helloWorld: function () {
            app.dialog.alert('Hello World!');
        },
    },
    routes: routes,
    vi: {
        placementId: 'pltd4o7ibb9rc653x14',
    }

});



//window.addEventListener("resize", function () {
//    myChart.resize();

//});

function shifouzai(a, b) {
    var zaifou = false;
    for (var i = 0; i < a.length; i++) {
        if (a[i] == b) {
            zaifou = true;
            break;
        }
    }
    return zaifou;
}//判断b是否在a中

$$(document).on('click', '#shijuanlist a', function () {
    app.dialog.preloader();
});

$$(document).on('page:init', function (e, page) {
    //var page = e.detail.page;
    if (page.name === "shijuan") {
        var ids = [];
        var kaiguan = false;
        var zongshu = 0;
        var zuotishu = 0;
        var shijuanhao = page.$el.data("shijuanid");
        var mySwiper = new Swiper('.swiper-container');//幻灯片
        var thispage = page.$el;
        $$(thispage).find('.swiper-slide').each(function () {
            zongshu = zongshu + 1;
            if ($$(this).data("zhuangtai") != "weizuo") {
                zuotishu = zuotishu + 1;
            }
        });
        var z = (zuotishu / zongshu) * 100;
        page.app.progressbar.set("#wanchengdu", z);//设置进度条

        $$(thispage).on('change', 'input', function () {

            var timuitem = $$(this).parent().parent().parent().parent().parent();
            var iid = timuitem.attr("id");
            $$("#biaoqian" + iid).addClass("color-blue");
            if (kaiguan) {
                if (!shifouzai(ids, iid)) {
                    ids.push(iid);
                }
            }

            if (timuitem.data("zhuangtai") === 'weizuo') {
                zuotishu = zuotishu + 1;
                timuitem.data("zhuangtai", 'yizuo');
                var z = (zuotishu / zongshu) * 100;
                page.app.progressbar.set("#wanchengdu", z);
            } else {
                timuitem.data("zhuangtai", 'yizuo');
            }
            mySwiper.slideNext();
        });//点击选项

        $$('.sheet-modal').on('click', '.chip', function () {
            var index = $$(this).find("div").text() * 1 - 1;
            mySwiper.slideTo(index);
            app.sheet.close('.sheet-modal');
        });//点击导航栏的标签

        timer = setInterval(function () {

            var answers = [];
            $$(".swiper-slide").each(function () {
                if ($$(this).data("zhuangtai") === "yizuo") {
                    var answer = {};
                    answer["daan"] = $$(this).find(":checked").val();
                    answer["id"] = $$(this).find(":checked").attr('name');
                    answers.push(answer);

                }
            });

            if (answers.length > 0) {
                ids = [];
                kaiguan = true;

                app.request({
                    type: 'POST',
                    url: 'baocun',
                    data: { sjh: shijuanhao, das: JSON.stringify(answers) },
                    timeout: 10000,
                    error: function () {
                        kaiguan = false;

                        ids = [];
                        app.toast.create({
                            text: '答案保存失败，请检查网络。',
                            closeTimeout: 3000,
                            position: 'center'
                        }).open();
                    },
                    success: function (data) {
                        kaiguan = false;
                        if (data == "true") {
                            for (var i = 0; i < answers.length; i++) {
                                if (!shifouzai(ids, answers[i]["id"])) {
                                    $$("#" + answers[i]["id"]).data("zhuangtai", "yicun");
                                }
                            }
                        }
                        ids = [];
                    }
                });


            }

        }, 10000);//每隔5分钟保存一次答案

        $$(thispage).on('click', '#tijiao', function () {
            var tongguo = true;

            $$(".swiper-slide").each(function () {

                if ($$(this).data("zhuangtai") === "weizuo") {
                    tongguo = false;
                    return false;
                }
            });

            if (tongguo) {
                app.dialog.confirm('提交后将不能继续答题', '确定提交？', function () {
                    var answers = [];
                    $$(".swiper-slide").each(function () {
                        if ($$(this).data("zhuangtai") === "yizuo") {
                            var answer = {};
                            answer["daan"] = $$(this).find(":checked").val();
                            answer["id"] = $$(this).find(":checked").attr('name');
                            answers.push(answer);
                        }
                    });
                    app.dialog.preloader('提交中...');
                    app.request({
                        type: 'POST',
                        url: 'jiaojuan',
                        data: { sjh: shijuanhao, das: JSON.stringify(answers) },
                        timeout: 10000,
                        error: function () {
                            app.dialog.close();
                            app.toast.create({
                                text: '交卷失败，请检查网络。',
                                closeTimeout: 3000,
                                position: 'center'
                            }).open();
                        },
                        success: function (data) {
                            app.dialog.close();
                            $$("#" + shijuanhao).attr("href", "chakanshijuan?sjh=" + shijuanhao);
                            $$("#" + shijuanhao).find('.chip').removeClass("color-pink").addClass("color-blue").find("div").text("已提交");
                            var v = app.view.get(".view-main");

                            v.router.back({ ignoreCache: true });
                        }
                    });
                });


            }
            else {
                app.toast.create({
                    text: '还有题目未完成，请完成后再提交',
                    closeTimeout: 3000,
                    position: 'center'
                }).open();
            }

        })//提交试卷
    }

    if (page.name === "chakanshijuan") {

        var zongshu = 0;
        var zuotishu = 0;

        var mySwiper = new Swiper('.swiper-container');//幻灯片
        var thispage = page.$el;
        $$(thispage).find('.swiper-slide').each(function () {
            zongshu = zongshu + 1;
            if ($$(this).data("zhuangtai") != "weizuo") {
                zuotishu = zuotishu + 1;
            }
        });
        var z = (zuotishu / zongshu) * 100;

        page.app.progressbar.set("#wanchengdu", z);//设置进度条

        $$('.sheet-modal').on('click', '.chip', function () {
            var index = $$(this).find("div").text() * 1 - 1;
            mySwiper.slideTo(index);
            app.sheet.close('.sheet-modal');
        });//点击导航栏的标签
    }

})//打开试卷页

$$("#shijuanlist").find(".item-subtitle").each(function () {
    $$(this).text("发布时间:" + UnixToDate($$(this).text(), false, 8));
});

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

