var app = getApp();

var loading = false;

function formatNum(num) {
  var num = (num || 0).toString();
  var result = '';
  while (num.length > 3) {
    result = ',' + num.slice(-3) + result;
    num = num.slice(0, num.length - 3);
  }
  if (num) { result = num + result; }
  return result;
}

function limitStr(str, length){
  length = length ? length : 20;

  if(str && str.length > 20){
    return str.substr(0,20) + '...';
  }

  return str;
}

function formatUrl(dataList){
  if (!dataList || dataList.length == 0){
    return dataList;
  }

  var tempList = [];
  for (var i = 0; i < dataList.length; i++){
    var item = dataList[i];
    if (item.BookImage){
      item.BookImage = item.BookImage.replace('http://image-1.openbook.com.cn/', 'https://image-1.openbookscan.com.cn/');
    }

    if (item.HistorySales_Mix){
      item.HistorySales_Mix = formatNum(item.HistorySales_Mix);
    }

    if (item.MonthSales_Mix){
      item.MonthSales_Mix = formatNum(item.MonthSales_Mix);
    }
    
    item.Author = limitStr(item.Author);

    tempList.push(item);
  }
  return tempList;
}

Page({

  /**
   * 页面的初始数据
   */
  data: {
    contentHeight: '500rpx',
    cycleType: 1,
    bookTitle: '',
    bookId: 0,
    total: 0,
    pageIndex: 1,
    pageSize: 20,
    orderType: 1,//累计销量降序  2 月销量降序
    bookList: [],
    canLoadMore: false,
    loadText: '加载更多>>',
    expiredTime: '',
    fdheight: 500,
    fdx: 330,
    fdy: 600
  },

  identify: function(){
    this.setData({
      showModal: false
    });
    wx.navigateTo({
      url: '../register/more/validate',
    })
  },

  closeModol: function(){
    this.setData({
      showModal: false
    });
  },

  preventTouchMove: function(){

  },

  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    var that = this;
    that.setData({
      author: app.wx_author_name,
      simpleAuthor: app.wx_author_name.length > 5 ? app.wx_author_name.substr(0, 4) + '...' : app.wx_author_name,
      expiredTime: app.wx_expired_time,
      showModal: app.wx_push_notify == 1 ? true : false
    });

    var systemInfo = wx.getSystemInfoSync();
    var px = systemInfo.windowHeight - 99 / 750 * systemInfo.windowWidth - 60; 
    that.setData({
      contentHeight: px + 'px'
    });

    that.loadSearchData();
  },

  showAuthorName: function(){
    var authorName = this.data.author;
    if (authorName && authorName.length > 5){
      wx.showModal({
        title: '作者全称',
        content: authorName,
        showCancel: false
      })
    }
  },

  updateAuthor: function(authorName, authorId, callBack){
    var that = this;
    //发送设置请求
    app.smartPost({
      url: '/author/setauthor',
      data: { AuthorId: authorId, AuthorName: authorName },
      callBack: function(json){
        if(json.data.Status < 0){
          wx.showModal({
            title: '系统异常',
            content: json.data.Msg,
            showCancel: false
          })
          return;
        }

        app.wx_author_name = authorName;
        that.setData({
          author: app.wx_author_name,
          simpleAuthor: app.wx_author_name.length > 5 ? app.wx_author_name.substr(0, 4) + '...' : app.wx_author_name,
          expiredTime: app.wx_expired_time,
          showModal: app.wx_push_notify == 1 ? true : false,
          bookList: [],
          pageIndex: 1
        });

        callBack();
      }
    });
  },

  resetAuthor: function(){
    //判断是不是演示账号（后期会判断是否是出版单位账号）
    if (app.wx_role_type != 1){
      return;
    }

    wx.vibrateShort({
      complete: function(){
        //跳转到作者搜索页面
        wx.navigateTo({
          url: '../author/query',
        })
      }
    });
  },

  closeMsgModol: function(){
    this.setData({
      showMsgModal: false
    });
  },

  showMsgModol: function(){
    this.setData({
      showMsgModal: true
    });
  },

  
  loadSearchData: function(){
    var that = this;

    if (loading){
      return;
    }

    loading = true;

    wx.showLoading({
      title: 'loading...',
    })

    that.setData({
      canLoadMore: false,
      loadText: 'loading...'
    });

    app.smartPost({
      url: '/Book/Search',
      data: { BookTitle: that.data.bookTitle, CycleType: that.data.cycleType, OrderType: that.data.orderType, PageIndex: that.data.pageIndex, PageSize: that.data.pageSize  },
      callBack: function(json){
        loading = false;
        wx.hideLoading();
        if(json.data.Status > 0){
          //加载成功
          //如果第一页  直接赋值  如果不是  则追加
          if (that.data.pageIndex == 1) {
            that.setData({
              latestDate: json.data.Data.LatestDate,
              bookList: formatUrl(json.data.Data.List)
            });
          }else{
            var list = that.data.bookList.concat(formatUrl(json.data.Data.List));
            that.setData({
              latestDate: json.data.Data.LatestDate,
              bookList: list
            });
          }
          

          if (that.data.pageIndex == 1){
            that.setData({
              total: json.data.Data.Total
            });
          }

          //判断当前页是否是最后一页
          var totalPages = Math.ceil(that.data.total / that.data.pageSize);

          if (that.data.pageIndex == 1){
            that.setData({
              scrollTop: 0
            })
          }

          var canLoad;
          var text;
          if(totalPages <= that.data.pageIndex){
            canLoad = false;
            text = '没有更多了';
          }else{
            canLoad = true;
            text = '加载更多>>';
          }

          that.setData({
            canLoadMore: canLoad,
            loadText: text
          });
        }else{
          //加载失败
          that.setData({
            canLoadMore: true,
            loadText: json.data.Msg
          });
        }
      }
    });
  },

  feedback: function(){
    wx.navigateTo({
      url: '../feedback/feedback',
    })
  },

  loadMore: function(){
    var that = this;
    if (!that.data.canLoadMore){
      return;
    }

    that.setData({
      pageIndex: that.data.pageIndex + 1
    });

    that.loadSearchData();
  },

  selectOrderType: function(e){
    var that = this;

    if(loading){
      return;
    }

    that.setData({
      orderType: e.target.dataset.order,
      pageIndex: 1
    });

    that.loadSearchData();
  },

  selectCycleType: function(e){
    var that = this;
    that.setData({
      cycleType: e.target.dataset.cycle
    });

    that.loadSearchData();
  },

  clearInput: function () {
    var that = this;
    that.setData({
      bookTitle: "",
      pageIndex: 1
    });

    that.loadSearchData();
  },

  compareSearch: function(){
    wx.navigateTo({
      url: 'index'
    })
  },

  /**
   * 生命周期函数--监听页面初次渲染完成
   */
  onReady: function () {
    var that = this;
    //获取设备屏幕的高度
    //并计算可移动区域的高度
    wx.getSystemInfo({
      success: function(res) {
        var moveableHeight = res.windowHeight;
        //需要去掉的高度是166rpx  计算得到响应的px值
        // 216 / 750 * res.windowWidth;

        var bodyHeight = moveableHeight - 216 / 750 * res.windowWidth;

        that.setData({
          fdheight: bodyHeight,
          fdy: bodyHeight - 100,
          fdx: 600 / 750 * res.windowWidth
        });
      }
    })
  },

  /**
   * 生命周期函数--监听页面显示
   */
  onShow: function () {
    
  },

  /**
   * 生命周期函数--监听页面隐藏
   */
  onHide: function () {
  
  },

  /**
   * 生命周期函数--监听页面卸载
   */
  onUnload: function () {
  
  },

  /**
   * 页面相关事件处理函数--监听用户下拉动作
   */
  onPullDownRefresh: function () {
  
  },

  /**
   * 页面上拉触底事件的处理函数
   */
  onReachBottom: function () {
  
  },
  onShareAppMessage: function () {
    return {
      title: '作家邦',
      path: '/pages/login/login',
      imageUrl: '/content/images/phone.png'
    }
  }
})