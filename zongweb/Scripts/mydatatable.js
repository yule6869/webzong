(function ($) {
    $.fn.mydatatable = function (options) {
        var defaults = {
            //各种参数、各种属性
            url: '',
            canshu: {}
        };
        //options合并到defaults上,defaults继承了options上的各种属性和方法,将所有的赋值给endOptions
        var endOptions = $.extend(defaults, options);
        this.each(function () {
            //实现功能的代码
            var _yejiaotext;
            var _this = $(this);
            var _zongyema;
            var _dangqianye;
            var _yematext = "<ul class='pagination pagination-sm' style='margin-top: 5px; float: right;'>";
            function _xianshi(data) {
                if (data.result == "true") {
                    var zongshu = data.zs;
                    var kaishi = data.ks;
                    var jieshu = data.js;
                    var shujus = data.tms;
                    var _dangqianye = data.dqy;
                    _yejiaotext = "<span class='small'>显示第 " + kaishi + " 至 " + jieshu + " 项，共" + zongshu + "项</span>";
                    _this.after(_yejiaotext);
                    _zongyema = parseInt(zongshu / 20);
                    if (zongshu % 20 != 0) {
                        _zongyema++;
                    }
                    if (_zongyema <= 7) {
                        for (var i = 1; i <= _zongyema; i++) {
                            if (_dangqianye == i) {
                                _yematext = _yematext + "<li class='active'>" + i + "</li>";
                            }
                            else {
                                _yematext = _yematext + "<li>" + i + "</li>";
                            }
                        }
                        _yematext = _yematext + "</ul>";
                    }
                    else {
                        if (_dangqianye <= 4) {
                            _yematext = _yematext + "<li><span aria-hidden='true'>«</span></li >";
                            for (var i = 1; i <= 5; i++) {
                                if (_dangqianye == i) {
                                    _yematext = _yematext + "<li class='active'><a>" + i + "</a></li>";
                                }
                                else {
                                    _yematext = _yematext + "<li><a>" + i + "</a></li>";
                                }
                            }
                            _yematext = _yematext + "<li><a>...</a></li><li><a>" + _zongyema + "</a></li><li><span aria-hidden='true'>»</span></li></ul>";
                        }
                        else if (_dangqianye >= _zongyema - 3) {
                            _yematext = _yematext + "<li><span aria-hidden='true'>«</span></li ><li><a>...</a></li>";
                            for (var i = _zongyema - 4; i <= _zongyema; i++) {
                                if (_dangqianye == i) {
                                    _yematext = _yematext + "<li class='active'><a>" + i + "</a></li>";
                                }
                                else {
                                    _yematext = _yematext + "<li><a>" + i + "</a></li>";
                                }
                            }
                            _yematext = _yematext + "<li><a>" + _zongyema + "</a></li><li><span aria-hidden='true'>»</span></li></ul>";
                        }
                        else {
                            _yematext = _yematext + "<li><span aria-hidden='true'>«</span></li ><li>...</li>";
                            _yematext = _yematext + "<li><a>" + _dangqianye - 1 + "</a></li>";
                            _yematext = _yematext + "<li class='active'><a>" + _dangqianye + "</a></li>";
                            _yematext = _yematext + "<li><a>" + (_dangqianye + 1) + "</a></li><li><a>...</a></li><li><a>" + _zongyema + "</a></li><li><span aria-hidden='true'>»</span></li></ul>";
                        }

                    }
                    _this.after(_yematext);
                    
                    for (var i = 0; i < shujus.length; i++) {
                        var row = "<tr><td>" + shujus[i]._id + "</td><td>" + shujus[i]._timu + "</td><td>" + shujus[i]._leibie + "</td><td>" + shujus[i]._zuoticishu + "</td><td>" + shujus[i]._jiaoshiid + "</td><td>" + shujus[i]._id + "</td></tr>";
                        _this.find("tbody").append(row);
                    }
                }
            }

            layer.open({ type: 2, content: "加载中…", shadeClose: false });
            $.ajax({
                type: "POST",
                url: endOptions.url,
                data: endOptions.canshu,
                timeout: 10000, //超时时间：30秒
                traditional: true,
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    //TODO: 处理status， http status code，超时 408
                    // 注意：如果发生了错误，错误信息（第二个参数）除了得到null之外，还可能
                    //是"timeout", "error", "notmodified" 和 "parsererror"。
                    layer.closeAll();
                    swal("注意", "提交失败，请检查网络", "error");
                },
                success: function (data) {
                    layer.closeAll();
                    if (data.result == "true") {
                        _xianshi(data);
                    }
                    else {
                        swal("失败", "获取数据失败", "error");
                    }
                }
            });//获取数据
        });



    };

})(jQuery);