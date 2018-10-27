var app = getApp();

Page({

  /**
   * 页面的初始数据
   */
  data: {
    inputVal: '',
    bookList: []
  },

  showInput: function () {
    this.setData({
      inputShowed: true
    });
  },

  hideInput: function () {
    this.setData({
      inputVal: "",
      inputShowed: false
    });
  },

  search: function(){
    var name = this.data.inputVal;
    
    var pages = getCurrentPages();
    var prevPage = pages[pages.length - 2];
    prevPage.setData({
      bookTitle: name,
      pageIndex: 1
    })

    wx.navigateBack({
      delta: 1
    })

    prevPage.loadSearchData();
  },

  itemSearch: function(e){
    var id = e.currentTarget.dataset.id;
    var name = e.currentTarget.dataset.name;

    var pages = getCurrentPages();
    var prevPage = pages[pages.length - 2];
    prevPage.setData({
      bookTitle: name,
      bookId: id,
      pageIndex: 1
    })

    wx.navigateBack({
      delta: 1
    })

    prevPage.loadSearchData();
  },

  
  clearInput: function () {
    this.setData({
      inputVal: "",
      bookList: []
    });
  },

  inputTyping: function (e) {
    var that = this;

    if (that.data.outId) {
      clearTimeout(that.data.outId);
    }

    var outId = setTimeout(function () {
      if (!that.data.inputVal) {
        return;
      }

      //请求联想
      app.smartPost({
        url: '/Book/NameRelate',
        data: { BookTitle: that.data.inputVal },
        callBack: function (json) {
          if (json.data.Status > 0) {
            that.setData({
              bookList: json.data.Data.List
            })
          }
        }
      });

    }, 700)

    that.setData({
      inputVal: e.detail.value,
      outId: outId
    });


  },

  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    
  },

  /**
   * 生命周期函数--监听页面初次渲染完成
   */
  onReady: function () {
    
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

  /**
   * 用户点击右上角分享
   */
  onShareAppMessage: function () {
  
  }
})