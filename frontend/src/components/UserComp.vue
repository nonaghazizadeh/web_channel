<template>
    <div>
        <b-spinner class="channel-loader" v-if="totalLoading" label="Spinning"></b-spinner>

        <div v-else dir="rtl" class="channel-page container-fluid">
            <div class="row">
                <div class="col-1 info">
                    <img src = "../assets/images/avatar.png" class = "rounded-circle avatar" width = "40" height = "40">
                    <div class="position-absolute exit-icon-container" >
                        <font-awesome-icon icon="fa-solid fa-arrow-right-from-bracket" class="exit-icon" @click="exit()" />
                    </div> 
                </div>
                <div class="col content no-float">
                    <div class="row top-content">
                        <div class="col-2 text-right">
                            <h5 class="channel-info">حساب کاربری</h5>        
                        </div>
                        <div class="col">
                        </div>
                        <div class="col-1">
                            <router-link to="/channel" class="close-icon">
                                <font-awesome-icon icon="fa-solid fa-close" class="channel-info-icon close-icon" />
                            </router-link>
                        </div>
                    </div>

                    <div class="row bottom-content mt-3 px-5">
                        <div class="col-2">
                                <b-form-input 
                                type="number" 
                                placeholder="کد تایید" 
                                v-model="verifyCode" 
                                ></b-form-input>
                        </div>
                        <div class="col-2">
                            <b-button variant="secondary" @click="verify()">
                                <b-spinner v-if="verifyLoading" label="Spinning"></b-spinner>
                                <span v-else>
                                    تایید  
                                </span>   
                            </b-button>
                        </div>
                        <div class="col-2">
                            <b-button variant="secondary" @click="deleteUser()">
                                حذف حساب کاربری     
                            </b-button>
                        </div>
                    </div>  
                    <div class="row center-add-content">
                        <div class="col">
                            <div class="row center-add-content px-5">
                                <div class="col-6">
                                    <b-form-input type="text" placeholder="نام و نام خانوادگی" v-model="name"></b-form-input>
                                </div>
                                <div class="col-6">
                                    <b-form-file
                                        class=""
                                        v-model="pic"
                                        placeholder="" 
                                    ></b-form-file>
                                </div>
                            </div>
                            <div class="row center-add-content px-5 py-1">
                                <div class="col-6">
                                    <b-form-input type="email" placeholder="ایمیل" v-model="email"></b-form-input>
                                </div>
                                <div class="col-6">
                                    <b-form-input type="number" placeholder="تلفن‌همراه" v-model="phone"></b-form-input>
                                </div>
                            </div>
                            <div class="row center-add-content px-5 py-1">
                                <div class="col-6">
                                    <b-form-input type="number" placeholder="شماره کارت" v-model="nationalId"></b-form-input>
                                </div>
                            </div>
                            <div class="row center-2-add-content px-5 py-1">
                                <div class="col-12">
                                    <b-form-textarea
                                        id="textarea-rows"
                                        placeholder="بیوگرافی خود را وارد کنید"
                                        rows="4"
                                        v-model="bio"
                                    ></b-form-textarea>  
                                </div>
                            </div>
                            <div class="row cetner-3-add-content px-5">
                                <div class="col-4">
                                    <b-input-group append="تومان">
                                        <b-form-input :placeholder="srcWallet" disabled>
                                        </b-form-input>
                                    </b-input-group>
                                </div>
                                <div class="col-2">
                                        <b-form-input type="number" placeholder="مبلغ شارژ" v-model="chargeMoney"></b-form-input>
                                </div>
                                <div class="col-2">
                                    <b-button variant="secondary" @click="chargeWallet()">
                                        <b-spinner v-if="walletLoading" label="Spinning"></b-spinner>
                                        <span v-else>
                                            شارژ کیف پول   
                                        </span>   
                                    </b-button>
                                </div>
                                <div class="col-2">
                                        <b-form-input type="number" placeholder="مبلغ برداشتی" v-model="walletMoney"></b-form-input>
                                    </div>
                                <div class="col-2">
                                    <b-button variant="secondary" @click="withDraw()">
                                        <b-spinner v-if="walletLoading" label="Spinning"></b-spinner>
                                        <span v-else>
                                            برداشت کیف پول   
                                        </span>
                                    </b-button>
                                </div>
                            </div>                            
                            <div class="row bottom-content">
                                <div class="col-10">
                                </div>
                                <div class="col-2">
                                    <b-button variant="secondary" @click="updateUser()">
                                        <b-spinner v-if="walletLoading || userLoading" label="Spinning"></b-spinner>
                                        <span v-else>
                                            ذخیره   
                                        </span>  
                                    </b-button>
                                </div>
                            </div>
                        </div>
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
    created(){
        this.createdMethods();
    },
    data(){
        return {
            totalLoading: false,
            walletLoading: false,
            userLoading: false,
            isJoin: true,
            isUser: false,
            modalShow: false,
            addChannelShow: false,
            walletMoney: '',
            chargeMoney: '',
            name: '',
            pic: '',
            email: '',
            phone: '',
            nationalId: '',
            bio: '',
            srcWallet: '',
            notDeletedYet: true,
            verifyLoading: false,
            verifyCode: ''
        }
    },
    methods: {
        verify(){
            console.log('hiiii')
            this.verifyLoading = true;

            let api= "http://79.127.54.112:5000/Profile/VerifyRemoveProfile/" + this.verifyCode
            const data = null
            const headers = {
                'X-Auth-Token': localStorage.getItem('token'),
            }
            Vue.axios.delete(api, {headers: headers, data: data })
            .then(response => {
                console.log(response)
                this.verifyLoading = false
                this.$router.push('/')
            })
            .catch((e) => {
                console.log(e)
                this.$bvToast.toast(e.response.data.message, {title: 'پیام خطا',autoHideDelay: 5000, appendToast: true})
                this.verifyLoading = false;
            })

        },


        createdMethods(){
            this.getUser()
        },
        getUser(){
            this.totalLoading = true;
            let api = "http://79.127.54.112:5000/Profile/GetProfile"
            Vue.axios.get(api, {
            headers: {
                'X-Auth-Token': localStorage.getItem('token')
            }
            })
            .then(response => {
                console.log(response)
                this.name = response.data.message.name;
                this.email = response.data.message.email;
                this.phone = response.data.message.phoneNumber;
                this.nationalId = response.data.message.cardNumber;
                this.bio = response.data.message.biography;
                this.getWallet()
            })
            .catch((e) => {
                console.log(e)
                this.$bvToast.toast(e.response.data.message, {title: 'پیام',autoHideDelay: 5000, appendToast: true})
                this.totalLoading = false;
            })
        },
        getWallet(){
            let walletApi = "http://79.127.54.112:5000/Wallet/GetWallet"
            Vue.axios.get(walletApi, {
            headers: {
                'X-Auth-Token': localStorage.getItem('token')
            }
            })
            .then(response => {
                console.log(response)
                this.srcWallet = response.data.message;
                this.totalLoading = false;
            })
            .catch((e) => {
                console.log(e)
                this.$bvToast.toast(e.response.data.message, {title: 'پیام',autoHideDelay: 5000, appendToast: true})
                this.totalLoading = false;
            })
        },

        chargeWallet(){
            this.totalLoading = true;
            let api= "http://79.127.54.112:5000/Wallet/ChargeWallet"
            const data = {
                Amount: this.chargeMoney
            }
            Vue.axios.put(api, data,{
                headers: {
                'X-Auth-Token': localStorage.getItem('token')
            }
            })
            .then(response => {
                console.log(response)
                window.location.reload()
            })
            .catch((e) => {
                console.log(e)
                this.$bvToast.toast(e.response.data.message, {title: 'پیام',autoHideDelay: 5000, appendToast: true})
                this.walletMoney = ''
                this.totalLoading = false;
            })

        },

        deleteUser(){
            let api = "http://79.127.54.112:5000/Profile/RemoveProfile";
            const data = null
            const headers = {
                'X-Auth-Token': localStorage.getItem('token'),
            }
            Vue.axios.delete(api, {headers: headers, data: data })
			.then(response => {
                console.log(response)
                this.notDeletedYet = false
                this.$bvToast.toast(response.data.message, {title: 'پیام',autoHideDelay: 5000, appendToast: true})

            }).catch((e) => {
                console.log(e)
                this.$bvToast.toast(e.response.data.message, {title: 'پیام خطا',autoHideDelay: 5000, appendToast: true})
            })
        },
        updateUser(){
            let api = "http://79.127.54.112:5000/Profile/AddProfile"
            this.userLoading = true;
            const data = {
                Name: this.name,
                Email: this.email,
                PhoneNumber: this.phone,
                CardNumber: this.nationalId,
                Biography: this.bio,
            }
			Vue.axios.post(api, data,{
            headers: {
                'X-Auth-Token': localStorage.getItem('token')
            }
            })
			.then(response => {
                console.log(response)
                let image_api = "http://79.127.54.112:5000/Profile/AddProfilePic"
                let formData = new FormData();
                formData.append('file', this.pic);
                Vue.axios.post(image_api, formData,{
                    headers: {
                        'X-Auth-Token': localStorage.getItem('token')
                    }
                }).then((response) => {
                    console.log(response)
                    this.userLoading = false;
                    this.$router.push('/channel')
                }).catch((e) => {
                    console.log(e)
                    this.$bvToast.toast(e.response.data.message, {title: 'پیام خطا',autoHideDelay: 5000, appendToast: true})
                    this.userLoading = false;
                })
            }).catch((e) => {
                console.log(e)
                this.$bvToast.toast(e.response.data.message, {title: 'پیام خطا',autoHideDelay: 5000, appendToast: true})
                this.loading = false;
            })

            this.$router.push('/channel')
        },
        exit(){
            localStorage.removeItem('token')
            this.$router.push('/')
        },
        withDraw(){
            this.walletLoading = true;
            let api = "http://79.127.54.112:5000/Wallet/Withdraw";
            const data = {
                Amount: this.walletMoney
            };
            Vue.axios.put(api, data,{
            headers: {
                'X-Auth-Token': localStorage.getItem('token')
            }
            })
			.then(response => {
                console.log(response)
                window.location.reload()
                
            }).catch((e) => {
                console.log(e)
                this.$bvToast.toast(e.response.data.message, {title: 'پیام',autoHideDelay: 5000, appendToast: true})
                this.walletMoney = ''
                this.walletLoading = false;
            })
        },
    }

}
</script>

<style scoped>
.channel-loader{
  position: fixed;
  z-index: 1031;
  top: 50%;
  left: 50%;
}
.input-content-file{
    margin-right: 50px !important;
}
.exit-icon-container{
    bottom:0;
}
.exit-icon{
    margin-right: 35px;
}
.avatar {
    margin-top: 20px;
}
.channel-page{
    height: 100vh;
}
.info{
    background-color: rgb(226, 226, 226);
}
.sidebar{
    background-color: white;
}
.content{
    background-color: white;
}
.row{
    height: 100%;
}
.search-icon {
    padding: 0.8rem 0.75rem !important;
    border-top-right-radius: 0px !important;
    border-bottom-right-radius: 0px !important;

}
.input-group:not(.has-validation) > :not(:last-child):not(.dropdown-toggle):not(.dropdown-menu):not(.form-floating) {
    border-top-left-radius: 0 !important;
    border-bottom-left-radius: 0 !important;
    border-top-right-radius: 10px !important;
    border-bottom-right-radius: 10px !important;
}
.channel-list{
    border-top-left-radius: 5px !important;
    border-bottom-left-radius: 5px !important;
    border-top-right-radius: 5px !important;
    border-bottom-right-radius: 5px !important;
}
.list-group-item.active {
    background-color: rgb(226, 226, 226);
    border-color: rgb(226, 226, 226);
    color: black;
}
.top-content{
    height: 10%;
    background-color: white;
}
.center-add-content{
    height: 30%;
}
.center-2-add-content{
    height: 55%;
}
.cetner-3-add-content{
    height: 35%;
}
.bottom-content {
    height: 10%;

}
.channel-image{
    margin-top: 20px;
}
.channel-info {
    margin-top: 25px;
}
.channel-info-icon {
    margin-top: 25px;
}
.sidebar-top {
    font-size: 20px;
    text-align: right;
    margin-right: 5px;
    font-weight: bold;
}
.add-icon{
    margin-right: 110px;
}
.input-group-text{
    border-top-right-radius: 0 !important;
    border-bottom-right-radius: 0 !important;
    border-top-left-radius: 10px !important;
    border-bottom-left-radius: 10px !important;
}
.like-icon{
    padding-bottom:0.09rem;
}
.user-title{
    font-weight: bold;
    font-size: 25px;
}
.delete-user-button{
    margin-left: 50px;
    text-align: left;
}
</style>