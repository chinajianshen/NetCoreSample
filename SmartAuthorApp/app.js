//app.js

var st = require('utils/smart_token.js');

App({
  calc_token: function (data, appkey, tick, sessionid, version) {
    return st.calc_token(data, appkey, tick, sessionid, version);
  },

  getcurrenttick: function () {
    return st.getcurrenttick();
    //dsajhf
  },

  wx_st_domain: 'https://test.openbookscan.com.cn',
  wx_st_appkey: 'Smart.Wx',
  wx_st_sid: '',
  wx_st_version: '0.0.2',
  wx_push_notify: 0,
  wx_expired_time: '',
  wx_expired_days: 365,
  wx_author_name: '',
  wx_role_type: '',

  smartRequest: function (url, data, method, callBack) {
    var that = this;
    var header = that.requestHeader(data);

    wx.request({
      url: that.wx_st_domain + url,
      data: data,
      header: header,
      method: method,
      success: function (data) {
        callBack(data);
      }
    });
  },

  smartAbortUpload: function () {
    var that = this;
    if (that.uploadTask) {
      that.uploadTask.abort();
    }
  },

  smartUpload: function (pams) {
    /*
    pams: {
      url: '',
      filePath: '',
      name: '',
      formData: '',
      callBack:  function(json){

      },
      progress: function(percent){
        //百分比
      },

    }
    */

    var that = this;
    var header = that.requestHeader(pams.formData, true);
    that.uploadTask = wx.uploadFile({
      url: that.wx_st_domain + pams.url, //仅为示例，非真实的接口地址
      header: header,
      filePath: pams.filePath,
      name: pams.name,
      formData: pams.formData,
      success: function (res) {
        that.uploadTask = undefined;
        var data = res.data
        try {
          pams.callBack(JSON.parse(data));
        } catch (e) {
          pams.callBack(data);
        }

      }
    })

    if (pams.progress) {
      that.uploadTask.onProgressUpdate((res) => {
        pams.progress(res.progress);
      })
    }



  },

  requestHeader: function (data, isUpload) {
    var that = this;
    var tick = st.getcurrenttick();
    data = data ? data : {};
    var token = st.calc_token(data, that.wx_st_appkey, tick, that.wx_st_sid, that.wx_st_version);
    var header = {
      wx_st_appkey: that.wx_st_appkey,
      wx_st_sid: that.wx_st_sid,
      wx_st_version: that.wx_st_version,
      wx_st_tick: tick,
      wx_st_token: token,
      'content-type': 'application/x-www-form-urlencoded'
    };

    if (isUpload) {
      header['content-type'] = 'multipart/form-data';
    }

    return header;
  },

  smartGet: function (pams) {
    this.smartRequest(pams.url, pams.data, 'GET', pams.callBack);
  },

  smartPost: function (pams) {
    this.smartRequest(pams.url, pams.data, 'POST', pams.callBack);
  },

  onLaunch: function () {

    // wx.removeStorage({
    //   key: 'session'
    // })

    // 展示本地存储能力
    // var logs = wx.getStorageSync('logs') || []
    // logs.unshift(Date.now())
    // wx.setStorageSync('logs', logs)

    // // 登录
    // wx.login({
    //   success: res => {
    //     // 发送 res.code 到后台换取 openId, sessionKey, unionId
    //     console.log(res.code);
    //   }
    // })
    // // 获取用户信息
    // wx.getSetting({
    //   success: res => {
    //     if (res.authSetting['scope.userInfo']) {
    //       // 已经授权，可以直接调用 getUserInfo 获取头像昵称，不会弹框
    //       wx.getUserInfo({
    //         success: res => {
    //           // 可以将 res 发送给后台解码出 unionId
    //           this.globalData.userInfo = res.userInfo

    //           // 由于 getUserInfo 是网络请求，可能会在 Page.onLoad 之后才返回
    //           // 所以此处加入 callback 以防止这种情况
    //           if (this.userInfoReadyCallback) {
    //             this.userInfoReadyCallback(res)
    //           }
    //         }
    //       })
    //     }
    //   }
    // })
  },
  globalData: {
    userInfo: null
  },
  onError: function(){
    wx.hideLoading();
    wx.showModal({
      title: '错误',
      content: '对不起，程序出现异常，请稍后访问或者反馈给客服',
      showCancel: false
    })
  }
})