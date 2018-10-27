var app = getApp();
var wxCharts = require('../../content/venders/wxcharts.js');
import * as echarts from '../../content/venders/echarts.js';
import geoJson from '../../content/venders/mapData.js';


var lineChart = null;
var columnChart = null;
var areaData = null;

var limitLength = function (arg, length) {
  if (arg == null) {
    return '-';
  }

  if (length == undefined) {
    length = 22;
  }

  if (arg.length < length) {
    return arg;
  }

  return arg.substr(0, length) + '...';
}

function proNull(arg, part){
  if(part == undefined){
    part = 0;
  }

  if(arg != undefined && arg != null){
    return arg;
  }

  return part;
}

function limitLength(arg, length){
  if (!length){
    length = 7;
  }

  if (!arg || arg.length <= length){
    return arg;
  }

  return (arg + '').substr(0, length - 1) + '...';
}


Page({

  /**
   * 页面的初始数据
   */
  data: {
    fdheight: 500,
    fdx: 330,
    fdy: 600
  },

  touchHandler: function (e) {
    lineChart.scrollStart(e);
  },

  touchHandlerCol: function (e) {
    columnChart.scrollStart(e);
  },

  moveHandler: function (e) {
    lineChart.scroll(e);
  },

  moveHandlerCol: function (e) {
    columnChart.scroll(e);
  },

  touchEndHandler: function (e) {
    lineChart.scrollEnd(e);
    lineChart.showToolTip(e, {
      format: function (item, category) {
        return category + ' ' + item.name + ':' + item.data
      }
    });
  },

  touchEndHandlerCol: function (e) {
    columnChart.scrollEnd(e);
    columnChart.showToolTip(e, {
      format: function (item, category) {
        return category + ' ' + item.name + ':' + item.data
      }
    });
  },

  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function(options) {
    var bookId = options.id;
    this.setData({
      bookId: bookId
    });
  },

  feedback: function () {
    wx.navigateTo({
      url: '../feedback/feedback',
    })
  },

  iniLineChart: function(salesData){
    var windowWidth = 320;
    try {
      var res = wx.getSystemInfoSync();
      windowWidth = res.windowWidth;
    } catch (e) {
      console.error('getSystemInfoSync failed!');
    }

    var data1 = [];
    var data2 = [];
    var data3 = [];
    var categories = [];

    var maxValue = 0;
    for (var i = 0; i < salesData.length; i++)
    {
      var item = salesData[i];
      categories.push(item.SaleDate);
      var num1 = proNull(item.MonthSales_Mix);
      var num2 = proNull(item.MonthSales_Loc);
      var num3 = proNull(item.MonthSales_Web);
      data1.push(num1);
      data2.push(num2);
      data3.push(num3);

      maxValue = Math.max(maxValue, num1, num2, num3);
    }

    
    if (maxValue == 0){
      maxValue = 10;
    }

    lineChart = new wxCharts({
      canvasId: 'lineCanvas',
      type: 'line',
      categories: categories,
      animation: true,
      // background: '#f5f5f5',
      series: [{
        name: '总体',
        data: data1
      }, {
        name: '实体店',
        data: data2
      }, {
        name: '网店',
        data: data3
      }],
      xAxis: {
        disableGrid: true
      },
      yAxis: {
        title: '销量',
        format: function (val) {
          return val.toFixed(0);
        },
        min: 0,
        max: maxValue
      },
      width: windowWidth,
      height: 150,
      dataLabel: false,
      dataPointShape: true,
      enableScroll: true,
      extra: {
        lineStyle: 'curve'
      }
    });
  },

  initMapChart: function (areaData) {
    var that = this;

    var windowWidth = 320;
    try {
      var res = wx.getSystemInfoSync();
      windowWidth = res.windowWidth;
    } catch (e) {
      console.error('getSystemInfoSync failed!');
    }

    //按销量从大到小排序
    areaData.sort(function(item1, item2){
      return item2.value - item1.value;
    });

    var data = [];
    var categories = [];
    var maxValue = 0;
    for (var i = 0; i < areaData.length; i++) {
      var item = areaData[i];
      categories.push(item.name);
      var num = proNull(item.value);
      data.push(num);

      maxValue = Math.max(maxValue, num);
    }
    if (maxValue == 0) {
      maxValue = 10;
    }
    
    columnChart = new wxCharts({
      canvasId: 'columnCanvas',
      type: 'column',
      categories: categories,
      animation: true,
      // background: '#f5f5f5',
      series: [{
        name: '销量',
        data: data
      }],
      xAxis: {
        disableGrid: false,
        type: 'calibration'
      },
      yAxis: {
        title: '地区销量',
        format: function (val) {
          return val.toFixed(0);
        },
        min: 0,
        max: maxValue
      },
      width: windowWidth,
      height: 200,
      dataPointShape: true,
      enableScroll: true,
      extra: {
        column: {
          width: 30
        }
      }
    });
  },

  /**
   * 生命周期函数--监听页面初次渲染完成
   */
  onReady: function() {
    var that = this;
    this.ecComponent = this.selectComponent('#mychart-dom-area');
    wx.setNavigationBarTitle({
      title: '图书详情'
    })

    //加载数据
    wx.showLoading({
      title: '加载中...',
    })

    //获取设备屏幕的高度
    //并计算可移动区域的高度
    wx.getSystemInfo({
      success: function (res) {
        var moveableHeight = res.windowHeight;
        //需要去掉的高度是166rpx  计算得到响应的px值
        // 216 / 750 * res.windowWidth;

        var bodyHeight = moveableHeight - 116 / 750 * res.windowWidth;

        that.setData({
          fdheight: bodyHeight,
          fdy: bodyHeight - 100,
          fdx: 600 / 750 * res.windowWidth
        });
      }
    })

    app.smartPost({
      url: '/Book/RinkingData',
      data: { BookID: that.data.bookId },
      callBack: function (json) {
        wx.hideLoading();
        if (json.data.Status < 0) {
          wx.showModal({
            content: json.data.Msg,
            showCancel: false
          });
          return;
        }

        var rinkingData = [];
        var kindNames = [];
        if (json.data.Data.RinkingData && json.data.Data.RinkingData.length > 0){
          var row = json.data.Data.RinkingData[0];
          for(var i = 0; i < 4; i++){
            var index = i + 1;
            if (row["Kind" + index + "Name"]){
              kindNames.push({ kindId: row["Kind" + index], kindName: limitLength(row["Kind" + index + "Name"]) });

              rinkingData.push({ id: row["Kind" + index], name: row["Kind" + index + "Name"], value: row["R" + index], breed: row["Breed" + index] });
            }
          }
        }

        var infoData = json.data.Data.InfoData[0];
        var simpleTitle = limitLength(infoData.Title);
        var simpleAuthor = limitLength(infoData.Author);
        that.setData({
          simpleTitle: simpleTitle,
          simpleAuthor: simpleAuthor
        });


        infoData.BookImage = infoData.BookImage ? infoData.BookImage.replace('http://image-1.openbook.com.cn/', 'https://image-1.openbookscan.com.cn/') : infoData.BookImage;

        that.setData({
          infoData: infoData,
          rinkingData: rinkingData,
          kindNames: kindNames,
          saleDate: json.data.Data.SaleDate
        });

        //判断是否是一号多数
        if (json.data.Data.InfoData[0].isMultiBook == 1){
          wx.showModal({
            title: '提示',
            content: '一号多书没有在榜排名。',
            showCancel: false
          })
        }

        //绘制折线图
        that.iniLineChart(json.data.Data.SalesData);

        //绘制中国地图
        var areaData = [];
        if (json.data.Data.AreaData && json.data.Data.AreaData.length > 0){
          areaData = json.data.Data.AreaData;
        }
        that.initMapChart(areaData);
      }
    });
  },

  showMsg: function(e){
    var msg = e.target.dataset.msg;
    wx.showModal({
      title: '注释',
      content: msg,
      showCancel: false
    })
  },

  showdetail: function(e){
    wx.showModal({
      title: '详细信息',
      content: e.target.dataset.detail,
      showCancel: false
    })
  },

  /**
   * 生命周期函数--监听页面显示
   */
  onShow: function() {
    
  },

  

  /**
   * 生命周期函数--监听页面隐藏
   */
  onHide: function() {

  },

  /**
   * 生命周期函数--监听页面卸载
   */
  onUnload: function() {

  },

  /**
   * 页面相关事件处理函数--监听用户下拉动作
   */
  onPullDownRefresh: function() {

  },

  /**
   * 页面上拉触底事件的处理函数
   */
  onReachBottom: function() {

  },

  /**
   * 用户点击右上角分享
   */
  onShareAppMessage: function() {

  }
})