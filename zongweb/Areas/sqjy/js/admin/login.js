$(document).ready(function () {

    $("input").focus(function () {
        $(".nousername").hide();
    });

    $("button").click(function () {
        var username = $("#username").val().trim();
        var password = $("#password").val().trim();
        if (username == "") {
            $(".loginmsg").text("用户名不能为空");
            $(".nousername").show();
            $("#username").val("");
            return;

        }
        if (password == "") {
            $(".loginmsg").text("密码不能为空");
            $(".nousername").show();
            $("#mima").val("");
            return;
        }
        layer.open({ type: 2, content: "提交中…", shadeClose: false });
        $.ajax({
            type: "POST",
            url: "Login",
            data: { un: username, mm: password },
            timeout: 10000, //超时时间：30秒
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //TODO: 处理status， http status code，超时 408
                // 注意：如果发生了错误，错误信息（第二个参数）除了得到null之外，还可能
                //是"timeout", "error", "notmodified" 和 "parsererror"。
                layer.closeAll();
                $(".loginmsg").text("通讯失败,请检查网络");
                $(".nousername").show();

            },
            success: function (data) {
                if (data == "true") {
                    layer.closeAll();
                    window.location.href = "xinwenguanli";

                }
                else {
                    layer.closeAll();
                    $(".loginmsg").text("用户名或密码错误，请重新输入");
                    $(".nousername").show();
                }
            }
        });

    });
});