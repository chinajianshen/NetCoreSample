var app = getApp();

Page({

  /**
   * 页面的初始数据
   */
  data: {
    realName: '',
    phone: '',
    mscode: '',
    hasSendMscode: false,
    sendMsBtn: {
      text: '发送验证码',
      cls: 'smart-vcode-btn',
      enabled: true
    },
    postBtn: {
      cls: 'smart-regist-btn'
    },
    sendPhone: '' //发送验证码的手机号
  },

  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function(options) {
    wx.setNavigationBarTitle({
      title: '注册'
    })

    this.setData({
      hasSendMscode: false
    });
  },
  realNameInput: function(e) {
    this.setData({
      realName: e.detail.value
    });
  },

  phoneInput: function(e) {
    this.setData({
      phone: e.detail.value
    });
  },
  mscodeInput: function(e) {
    this.setData({
      mscode: e.detail.value
    });
  },

  //发送手机验证码
  sendMsCode: function(e) {
    var that = this;

    if (!that.data.sendMsBtn.enabled) {
      return;
    }

    var phone = that.data.phone;
    var realName = that.data.realName;
    if (!realName || realName.replace(/\s*/, '').length == 0) {
      wx.showToast({
        title: '真实姓名不能为空，我们会根据您填写的信息，给您开通作家本人出版图书的查询功能。',
        icon: 'none',
        duration: 2000
      })
      return;
    }


    if (!phone || !/^1\d{10}$/.test(phone)) {
      wx.showToast({
        title: '请输入正确的手机号',
        icon: 'none',
        duration: 2000
      })
      return;
    }

    app.smartPost({
      url: '/auth/sendsmscode',
      data: {
        phone: phone
      },
      callBack: function(json) {
        if (json.data.Status == 0) {
          wx.showModal({
            content: '发送失败，请稍后再试！',
            showCancel: false
          });
          return;
        }

        wx.showToast({
          title: '发送成功',
          icon: 'success',
          duration: 2000
        })

        that.setData({
          sendPhone: phone,
          hasSendMscode: true
        });
        var total = 120;
        var intId = setInterval(function() {
          if (total > 0) {
            total--;
            that.setData({
              sendMsBtn: {
                text: '(' + total + 's)后重发',
                cls: 'smart-vcode-btn-disabled',
                enabled: false
              }
            });
          } else {
            clearInterval(intId);
            that.setData({
              sendMsBtn: {
                text: '发送验证码',
                cls: 'smart-vcode-btn',
                enabled: true
              }
            });
          }

        }, 1000);

      }
    });
  },

  //确认注册
  confirmRegist: function() {
    var that = this;
    var sendPhone = that.data.sendPhone;
    var phone = that.data.phone;
    var realName = that.data.realName;
    var mscode = that.data.mscode;
    var hasSendMscode = that.data.hasSendMscode;

    if (!realName || !phone || !mscode) {
      wx.showToast({
        title: '请填写相关信息',
        icon: 'none',
        duration: 2000
      })
      return;
    }

    if (!hasSendMscode) {
      wx.showToast({
        title: '请先获取验证码',
        icon: 'none',
        duration: 2000
      })
      return;
    }

    if (sendPhone != phone) {
      wx.showToast({
        title: '注册失败，请稍后再试',
        icon: 'none',
        duration: 2000
      })
      return;
    }

    wx.showLoading({
      title: '加载中...',
    })

    app.smartPost({
      url: '/auth/regist',
      data: {
        UserName: realName,
        LoginName: phone,
        SmsCode: mscode
      },
      callBack: function(json) {
        wx.hideLoading();
        if (json.data.Status > 0) {
          //注册成功
          wx.navigateTo({
            url: '../msg_success/msg_success',
          })
          return;
        }

        //注册失败  提示注册信息
        wx.showModal({
          content: json.data.Msg,
          showCancel: false
        });
      }
    });

  },

  /**
   * 生命周期函数--监听页面初次渲染完成
   */
  onReady: function() {

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