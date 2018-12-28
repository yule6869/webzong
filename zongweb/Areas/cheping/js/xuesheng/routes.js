var routes = [
    // Index page
    {
        path: '/',
        url: './shouye',
        name: 'home',
        on: {
            pageInit: function (e, page) {
                var myChart = echarts.init(document.getElementById('main'));
                myChart.setOption(option);
                var myChart2 = echarts.init(document.getElementById('main2'));
                myChart2.setOption(option2);
            },
            pageBeforeIn: function (event, page) {
                page.app.dialog.close();
            }

        }

    },
    // shijuan page
    {
        path: '/shijuan/',
        url: './shijuan',

        on: {

            pageBeforeIn: function (event, page) {
                page.app.dialog.close();
            },
            pageAfterOut: function (event, page) {
                clearInterval(timer);
            }

        }
    },
    // chakanshijuan page
    {
        path: '/chakanshijuan/',
        url: './chakanshijuan',
        on: {
            pageBeforeIn: function (event, page) {
                setTimeout(function () {
                    page.app.dialog.close();
                }, 1000);
                
                
            }

        }
    },
    {
        path: '/paiming/',
        url: './paiming',

        on: {

            pageBeforeIn: function (event, page) {
                page.app.dialog.close();
            }

        }

    },


];
