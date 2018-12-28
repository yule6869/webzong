var fashengshijian;
$(document).ready(function () {

    var ue = UE.getEditor('input0201');

    $("#date-picker").daterangepicker({
        singleDatePicker: true,
        showDropdowns: true,
        autoUpdateInput: true,
        
        "locale": {
            daysOfWeek: ["日", "一", "二", "三", "四", "五", "六"],
            monthNames: ["一月", "二月", "三月", "四月", "五月", "六月", "七月", "八月", "九月", "十月", "十一月", "十二月"]
        }
    },
        function (start, end, label) {
            fashengshijian = start.toISOString();
            fashengshijian = fashengshijian.substr(0, 10);
            console.log(fashengshijian);
        });

    $("#queding").click(function () {
        jianyan();
    })


});



function jianyan() {
    var xinwenbiaoti = $("#input001").val();
    var xinwenjianjie = UE.getEditor('input0201').getContentTxt().substr(0, 60) + "........";
    var xinwenleirong = UE.getEditor('input0201').getContent();
    var fenmiantupian;
    var leixing = $("#leixing option:selected").text();

    var bool = true;
    if (xinwenbiaoti == null || xinwenbiaoti == "") {
        $("#biaoti").addClass("has-error");
        $("#helpBlock").show();
        bool = false;
    }
    if (!UE.getEditor('input0201').getContent()) {
        $("#leirong").addClass("has-error");
        $("#helpBloc2k").show();
        bool = false;


    }
    else {
        var kaishi = xinwenleirong.search('<img');
        if (kaishi != -1) {
            kaishi = xinwenleirong.indexOf('src="', kaishi) + 5;
            var jieshu = xinwenleirong.indexOf('"', kaishi);
            fenmiantupian = xinwenleirong.substr(kaishi, jieshu - kaishi);
        }
        else {
            fenmiantupian = "/Areas/sqjy/img/xinwentupian/tidai.jpg";
        }



    }
    
    if (bool) {
        console.log(fashengshijian);
        
        $.ajax({
            type: "POST",
            url: "tianjiaxinwen",
            data: { xwbt: xinwenbiaoti, xwlx: leixing, xwlr: xinwenleirong, xwjj: xinwenjianjie, fmtp: fenmiantupian, fssj: fashengshijian},
            timeout: 10000, //超时时间：30秒
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //TODO: 处理status， http status code，超时 408
                // 注意：如果发生了错误，错误信息（第二个参数）除了得到null之外，还可能
                //是"timeout", "error", "notmodified" 和 "parsererror"。
             
             

            },
            success: function (data) {
                if (data == "true") {
                    swal({
                        title: "添加成功",
                        type: "success",
                        confirmButtonColor: "#DD6B55",
                        closeOnConfirm: false
                    },
                        function () {
                            window.location.replace("xinwenguanli");
                        });

                }
                else {
                  
                   
                }
            }
        });
    }
};