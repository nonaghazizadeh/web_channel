<template>
    <div>
      <div dir="rtl" class="channel-page container-fluid">
            <div class="row channel-container">
                <div class="col-1 info">
                    <img src = "../assets/images/avatar.png" class = "rounded-circle avatar" width = "40" height = "40">
                    <div class="position-absolute exit-icon-container" >
                        <router-link class="exit-link" to="/">
                            <font-awesome-icon icon="fa-solid fa-arrow-right-from-bracket" class="exit-icon" />
                        </router-link>
                    </div> 
                </div>
                <div class="col">
                    <div class="row top-content pt-4">
                        <div class="col-1"></div>
                        <div class="col-2 text-right">
                            <h5 class="channel-info">ایجاد کانال</h5>        
                        </div>
                        <div class="col">
                        </div>
                        <div class="col-1">
                            <router-link to="/channel" class="close-icon" v-show="!loading">
                                <font-awesome-icon icon="fa-solid fa-close" class="channel-info-icon close-icon" />
                            </router-link>
                        </div>
                    </div>
                    <div class="row px-5 py-3 mt-5">
                        <div class="col"></div>
                        <div class="col-5">
                            <b-form-input type="text" placeholder="نام کانال" v-model="channelName"></b-form-input>
                        </div>
                        <div class="col"></div>
                    </div>
                    <div>
                        <b-button variant="secondary" class="add-channel-button" @click="addChannel()">
                            <b-spinner v-if="loading">
                            </b-spinner>
                            <span v-else>
                            ایجاد کانال
                            </span>
                        </b-button>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>
  
<script>
import Vue from "vue";
import axios from "axios";
import VueAxios from "vue-axios";
Vue.use(VueAxios, axios);
export default {
    watch: {
        yesStatus: function (val) {
            if (val) {
                this.noStatus = false;
            }
        },
        noStatus: function (val) {
            if (val) {
                this.yesStatus = false;
            }
        }
    },
    data() {
        return {
            loading: false,
            yesStatus: true,
            noStatus: false,
            file1: null,
            channelName: '',
        }
    },
    methods:{
        addChannel(){
            let api = "http://79.127.54.112:5000/Channel/Add";
            const data = {
                ChannelName: this.channelName,
            };
            this.loading = true;
            Vue.axios.post(api, data,{
            headers: {
                'X-Auth-Token': localStorage.getItem('token')
            }
            })
			.then(response => {
                console.log(response)
                setTimeout(() => {
                    this.loading = false;
                    this.$router.push('/channel')
                }, 2000);
            }).catch((e) => {
                console.log(e)
                this.$bvToast.toast(e.response.data.message, {title: 'پیام خطا',autoHideDelay: 5000, appendToast: true})
                this.loading = false;
            })
        }
    }
}
</script>
  
  <style>
  .add-channel-button{
      margin-top: 20px;
      margin-right: 930px;
  }
  .exit-link{
      color: black !important;
  }
  .add-channel-title{
      margin-right: -250px;
  }
  .avatar {
      margin-top: 20px;
  }
  .channel-page{
      height: 100vh;
  }
  .content{
      background-color: white;
      overflow-y: scroll;
  }
  .info{
      background-color: rgb(226, 226, 226);
      /* position: fixed !important; */
  }
  .channel-container{
      height: 100%;
  }
  .exit-icon{
      margin-right: 35px;
  }
  .add-channel-title{
      font-weight: bold;
      font-size: 25px;
  }
  .remove-manager-button{
      margin-left: 5px;
  }
  .add-manager-first-button{
      margin-right: 44px;
  }
  
  .exit-icon-container {
      bottom:0;
  }
  .custom-file-label::after{
      content: "آپلود" !important;
  }
  .custom-file-label{
      text-align: right !important;
  }
  .top-content{
    height: 10%;
    background-color: white;
    border-bottom: 1px solid black;
}
.close-icon{
    color:black
}
  </style>