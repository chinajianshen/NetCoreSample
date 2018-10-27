var app = getApp();

Page({

  /**
   * 页面的初始数据
   */
  data: {
    quesContent: '',
    showModal: false, //是否开启弹出框
    stateTitle: '', //反馈结果标题
    stateMsg: ''  //反馈结果内容
  },

  inputContent: function(e){
    this.setData({
      quesContent: e.detail.value
    });
  },

  selectQues: function(e){
    var newId = e.target.dataset['id']
    var newTitle = e.target.dataset['title'];
    var newList = [];
    for (var i = 0; i < this.data.quesList.length; i++)
    {
      var ques = this.data.quesList[i];
      if (newId == ques.id)
      {
        ques.cls = 'ques-active';
      }else{
        ques.cls = 'ques-default';
      }
      newList.push(ques);
    }

    this.setData({
      quesList: newList,
      quesTitle: newTitle,
      quesId: newId
    });
  },

  //提交表单
  postForm: function(){
    var that = this;
    wx.showLoading({
      title: 'loading...',
    });

    app.smartPost({
      url: '/Auth/SaveAuthorFeedback',
      data: { FeedbackContent: that.data.quesContent, FeedbackType: that.data.quesId },
      callBack: function(json){
        wx.hideLoading();

        if(json.data.Status < 1){
          wx.showModal({
            title: '错误',
            content: json.data.Msg,
            showCancel: false
          })
          return;
        }

        wx.showModal({
          title: '成功',
          content: json.data.Msg,
          showCancel: false,
          success: function(res){
            if(res.confirm){
              wx.navigateBack({
                delta: 1
              })
            }
          }
        })
      }
    });


    
  },

  closeModol: function(){
    this.setData({
      showModal: false
    });
  },

  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    var that = this;
    wx.setNavigationBarTitle({
      title: '反馈'
    })

    //获取所有的反馈类型
    wx.showLoading({
      title: 'loading...',
    })

    app.smartPost({
      url: '/Auth/GetFeedbackTypes',
      callBack: function(json){
        wx.hideLoading();
        if(json.data.Status < 1){
          wx.showModal({
            title: '错误',
            content: json.data.Msg
          })
          return;
        }

        var listData = [];
        for(var i = 0; i < json.data.Data.length; i++){
          var item = json.data.Data[i];
          listData.push({ id: item.FeedbackTypeID, title: item.FeedbackTypeName, cls: i == 0 ? "ques-active" : "ques-default" });
        }

        that.setData({
          quesList: listData,
          quesId: listData[0].id,
          quesTitle: listData[0].title
        });
      }
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