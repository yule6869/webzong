(function ($) {
    $.fn.table = function (options) {
        var defaults = {
            //各种参数、各种属性
            evenRowClass: 'evenRow',
            oddRowClass: 'oddRow',
            curRowClass: 'curRow',
            eventType1: 'mouseover',
            eventType2: 'mouseout'
        };

        var endOptions = $.extend(defaults, options);

        this.each(function () {
            var _this = $(this);
            _this.find('tr:even').addClass(endOptions.evenRowClass);
            _this.find('tr:odd').addClass(endOptions.oddRowClass);
            //鼠标移入和移出，但实际开发中不直接使用mouseover这种方法
            /*$(this).find('tr').mouseover(function () {
                $(this).addClass(endOptions.curRowClass);
            }).mouseout(function () {
                $(this).removeClass(endOptions.curRowClass);
            });*/

            //实际开发中要用bian()方法绑定
            //因为用bind()方法绑定非常灵活，事件可以自己定义
            //mouseover mouseout...事件底层都是用bind()去实现的，mouseout 等只是快捷方式
            _this.find('tr').bind(endOptions.eventType1, function () {
                $(this).addClass(endOptions.curRowClass);
            });
            _this.find('tr').bind(endOptions.eventType2, function () {
                $(this).removeClass(endOptions.curRowClass);
            })
        });
    };
})(jQuery);