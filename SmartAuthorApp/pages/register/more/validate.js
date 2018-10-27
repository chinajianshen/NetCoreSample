var app = getApp();

Page({

  /**
   * 页面的初始数据
   */
  data: {
    showModal: false,
    completeModal: false,
    frontSrc: '/content/images/portrait.png',
    backSrc: '/content/images/emblem.png',
    frontSelected: false,
    backSelected: false
  },

  chooseimage: function (imageType) {
    var that = this;
    wx.showActionSheet({
      itemList: ['从相册中选择', '拍照'],
      itemColor: "#1C8EE9",
      success: function (res) {
        if (!res.cancel) {
          if (res.tapIndex == 0) {
            that.chooseWxImage('album', imageType)
          } else if (res.tapIndex == 1) {
            that.chooseWxImage('camera', imageType)
          }
        }
      }
    })

  },

  chooseWxImage: function (selectType, imageType) {
    var that = this;
    wx.chooseImage({
      sizeType: ['compressed'],
      sourceType: [selectType],
      count: 1,
      success: function (res) {
        if (imageType == 'front'){
          that.setData({
            frontSrc: res.tempFilePaths[0],
            frontSelected: true
          })
        }else{
          that.setData({
            backSrc: res.tempFilePaths[0],
            backSelected: true
          })
        }
      }
    })
  },

  //拍摄人像页
  selectFront: function(){
    this.chooseimage('front');
  },

  //拍摄国徽页
  selectBack: function(){
    this.chooseimage('back');
  },

  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    wx.setNavigationBarTitle({
      title: '完善认证资料',
    })
  },

  moreValidate: function(){
    var that = this;
    if(!that.data.frontSelected || !that.data.backSelected){
      return;
    }

    wx.showLoading({
      title: '正在上传',
    })
    //开始上传
    app.smartUpload({
      url: '/auth/UploadImage',
      filePath: that.data.frontSrc,
      name: 'Front',
      callBack: function(json){
        if (json.Status != 1){
          wx.hideLoading();
          wx.showModal({
            content: json.Msg,
            showCancel: false
          });
          return;
        }

        app.smartUpload({
          url: '/auth/UploadImage',
          filePath: that.data.backSrc,
          name: 'Back',
          callBack: function (json) {
            wx.hideLoading();

            if (json.Status != 1) {
              wx.showModal({
                content: json.Msg,
                showCancel: false
              });
              return;
            }

            //提示 确定后返回登录页
            wx.showModal({
              content: '恭喜您完成身份认证！我们将在5个工作日内短信通知您审核结果',
              showCancel: false,
              success: function(res){
                if(res.confirm){
                  wx.navigateBack({
                    delta: 3
                  })
                }
              }
            });
          }
        });
      }
    });
  },

  closeCompleteModol: function(){
    var that = this;
    that.setData({
      completeModal: false
    });
  },

  showDemo: function(){
    this.setData({
      showModal: true
    });
  },

  demoConfirm: function(){
    this.setData({
      showModal: false
    });
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
    app.smartAbortUpload();
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