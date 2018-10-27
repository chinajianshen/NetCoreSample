
var app = getApp();

Page({

  /**
   * 页面的初始数据
   */
  data: {
    inputVal: '',
    authorList: []
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

  itemSearch: function (e) {
    var id = e.currentTarget.dataset.id;
    var name = e.currentTarget.dataset.name;

    var pages = getCurrentPages();
    var prevPage = pages[pages.length - 2];
    prevPage.updateAuthor(name, id, function(){
      wx.navigateBack({
        delta: 1,
        success: function () {
          prevPage.loadSearchData();
        }
      })
    });
  },


  clearInput: function () {
    this.setData({
      inputVal: "",
      authorList: []
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
        url: '/Author/Search',
        data: { Author: that.data.inputVal },
        callBack: function (json) {
          if (json.data.Status > 0) {
            that.setData({
              authorList: json.data.Data
            })
          }
        }
      });

    }, 400)

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