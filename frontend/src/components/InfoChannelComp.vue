<template>
    <div>
        <b-spinner class="channel-loader" v-if="loading" label="Spinning"></b-spinner>
        <div v-else dir="rtl" class="channel-page container-fluid">
            <div class="row">
                <div class="col-1 info">
                    <img src = "../assets/images/avatar.png" class = "rounded-circle avatar" width = "40" height = "40" @click="goProfile()">
                    <div class="position-absolute exit-icon-container" >
                        <router-link to="/">
                            <font-awesome-icon icon="fa-solid fa-arrow-right-from-bracket" class="exit-icon" />
                        </router-link>
                    </div> 
                </div>
                <div v-if="!isUser" class="col content no-float">
                    <div class="row top-content">
                        <div class="col-1">
                            <img src="../assets/images/channelimg.jpeg" class = "rounded-circle channel-image" width = "35" height = "35">
                        </div>
                        <div class="col-2 text-right">
                            <h5 class="channel-info">نام کانال</h5>        
                        </div>
                        <div class="col">
                        </div>
                        <div class="col-1">
                            <router-link to="/channel" class="close-icon">
                                <font-awesome-icon icon="fa-solid fa-close" class="channel-info-icon close-icon" />
                            </router-link>
                        </div>
                    </div>
                    <div class="tabs my-5 py-5 ">
                        <b-tabs content-class="mt-5">
                            <b-tab title="تصویر کانال" active>
                                <div class="row">
                                    <div class="col"></div>
                                    <div class="col-3">
                                        <b-form-file
                                        v-model="channelImage"
                                        placeholder=""
                                        ></b-form-file>
                                    </div>
                                    <div class="col-2">
                                        <b-button variant="secondary" @click="addImage()">
                                            آپلود تصویر
                                        </b-button>
                                    </div>
                                    <div class="col-2">
                                        <b-button variant="secondary">
                                            <a class="image-link" :href="`http://79.127.54.112:5000/Channels/${this.channelId}.png`" target="_blank">
                                                تصویر کانال
                                            </a>
                                        </b-button>
                                    </div>
                                    <div class="col"></div>
                                </div>
                            </b-tab>
                            <b-tab title="ویرایش اطلاعات کانال">
                                <div class="row master-center-content">
                                    <div class="col-1"></div>
                                    <div class="col-2">
                                        <b-form-input type="text" placeholder="نام کانال" v-model="chName"></b-form-input>
                                    </div>
                                    <div class="col-7">
                                        <b-form-input type="text" placeholder="توضیحات کانال" v-model="desName"></b-form-input>
                                    </div>
                                    <div class="col-2">
                                        
                                        <b-button pill @click="editChannel()">
                                            <b-spinner v-if="updateLoading" label="Spinning"></b-spinner>
                                            <span v-else>
                                                تغییر 
                                            </span>
                                        </b-button>
                                    </div>
                                </div>
                            </b-tab>
                            <b-tab title="افزودن حق اشتراک">
                                <div class="row master-center-content">
                                    <div class="col-1"></div>
                                    <div class="col">
                                        <b-table :fields="fields" :items="subItems" sticky-header="200px">
                                        </b-table>
                                    </div>
                                    <div class="col-1"></div>
                                </div>

                                <div class="row master-center-2-content">
                                    <div class="col"></div>
                                    <div class="col-3">
                                        <b-form-select 
                                        v-model="addSubSelected"
                                        :disabled="yesStatus || !isAdmin"
                                        :options="subAddOptions">
                                        </b-form-select>
                                    </div>
                                    <div class="col-2">
                                        <b-form-input 
                                            type="number" 
                                            v-model="subFee"
                                            placeholder="هزینه اشتراک"
                                            :disabled="yesStatus || !isAdmin"
                                        ></b-form-input>
                                    </div>
                                    <div class="col-1">
                                        <b-button pill :disabled="yesStatus || !isAdmin" @click="addSubscription()">افزودن</b-button>
                                    </div>
                                    <div class="col"></div>
                                </div>
                            </b-tab>
                            <b-tab title="ویرایش حق اشتراک">
                                <div class="row master-center-content">
                                    <div class="col-1"></div>
                                    <div class="col">
                                        <b-table :fields="fields" :items="subItems" sticky-header="200px">
                                        </b-table>
                                    </div>
                                    <div class="col-1"></div>
                                </div>

                                <div class="row master-center-2-content">
                                    <div class="col"></div>
                                    <div class="col-3">
                                        <b-form-select 
                                        v-model="subSelectedUpdate"
                                        :options="subAddOptionsUpdate">
                                        </b-form-select>
                                    </div>
                                    <div class="col-2">
                                        <b-form-input 
                                            type="number" 
                                            v-model="subFeeUpdate"
                                            placeholder="هزینه اشتراک"
                                        ></b-form-input>
                                    </div>
                                    <div class="col-1">
                                        <b-button pill :disabled="yesStatus || !isAdmin" @click="editSubscription()">تغییر</b-button>
                                    </div>
                                    <div class="col"></div>
                                </div>
                            </b-tab>
                            <b-tab title="حذف کاربر">
                                
                                <div class="row master-center-content">
                                    <div class="col-1"></div>
                                    <div class="col">
                                        <b-table :fields="userFields" :items="userItems" sticky-header="200px">
                                    </b-table>
                                    </div>
                                    <div class="col-1"></div>
                                </div>

                                <div class="row master-center-2-content">
                                    <div class="col"></div>
                                    <div class="col-5">
                                        <b-form-select 
                                        v-model="delUserSelected"
                                        :options="userOptions">
                                        </b-form-select>
                                    </div>
                                    <div class="col-1">
                                        <b-button pill @click="removeMember()">حذف</b-button>
                                    </div>
                                    <div class="col"></div>
                                </div>
                            </b-tab>
                            <b-tab title="مدیر">
                                <div class="row master-center-content">
                                    <div class="col-1"></div>
                                    <div class="col">
                                        <b-table :fields="userFields" :items="adminItems" sticky-header="200px">
                                    </b-table>
                                    </div>
                                    <div class="col-1"></div>
                                </div>
                                <div class="row master-center-3-content">
                                    <div class="col"></div>
                                    <div class="col-3">
                                        <b-form-select 
                                        v-model="addAdmindSelected" 
                                        :disabled="!isAdmin"
                                        :options="userOptions"></b-form-select>
                                    </div>
                                <div class="col-2"></div>
                                <div class="col-2">
                                    <b-button pill :disabled="!isAdmin" @click="addAdmin()">افزودن مدیر</b-button>
                                </div>
                                <div class="col"></div>
                                </div>
                                <div class="row master-center-3-content mt-3">
                                    <div class="col"></div>
                                    <div class="col-3">
                                        <b-form-select 
                                        v-model="delAdmindSelected" 
                                        :disabled="!isAdmin"
                                        :options="adminOptions"></b-form-select>
                                    </div>
                                <div class="col-2"></div>
                                <div class="col-2">
                                    <b-button pill :disabled="!isAdmin" @click="removeAdmin()">حذف مدیر</b-button>
                                </div>
                                <div class="col"></div>
                                </div>
                                <div >
                                    <div v-for="manager in managersProfit" :key="manager.id" class="row mt-3">
                                        <div class="col"></div>
                                        <div class="col-3">
                                            <b-form-input type="text" placeholder="نام مدیر" disabled v-model="manager.name"></b-form-input>
                                        </div>
                                        <div class="col-2">
                                            <b-form-input type="number" placeholder="درصد سود" v-model="manager.profit" ></b-form-input>
                                        </div>
                                        <div class="col-2"></div>
                                        <div class="col"></div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col"></div>
                                    <div class="col-3"></div>
                                    <div class="col-2"></div>
                                    <div class="col-2">
                                    <b-button pill :disabled="!isAdmin" @click="setIncome()">تعیین درصد درآمد</b-button>
                                    </div>
                                    <div class="col"></div>
                                </div>
                            </b-tab>
                            <b-tab title="افزودن دسته‌بندی">
                                <div class="row master-center-content">
                                    <div class="col-1"></div>
                                    <div class="col">
                                        <b-table :fields="categoryFields" :items="categoryItems" sticky-header="200px">
                                        </b-table>
                                    </div>
                                    <div class="col-1"></div>
                                </div>

                                <div class="row master-center-2-content">
                                    <div class="col"></div>
                                    <div class="col-5">
                                        <b-form-input type="text" placeholder="نام دسته‌بندی" v-model="newCategory"></b-form-input>
                                    </div>
                                    <div class="col-1">
                                        <b-button pill @click="addCategory()">افزودن</b-button>
                                    </div>
                                    <div class="col"></div>
                                </div>
                            </b-tab>
                            <b-tab title="حذف دسته‌بندی">
                                <div class="row master-center-content">
                                    <div class="col-1"></div>
                                    <div class="col">
                                        <b-table :fields="categoryFields" :items="categoryItems" sticky-header="200px">
                                        </b-table>
                                    </div>
                                    <div class="col-1"></div>
                                </div>

                                <div class="row master-center-2-content">
                                    <div class="col"></div>
                                    <div class="col-5">
                                        <b-form-select 
                                            v-model="delCategorySelected" 
                                            :options="categoryOptions"
                                            :disabled="!isAdmin"
                                            ></b-form-select>
                                    </div>
                                    <div class="col-1">
                                        <b-button pill @click="deleteCategory()" :disabled="!isAdmin">حذف</b-button>
                                    </div>
                                    <div class="col"></div>
                                </div>
                            </b-tab>
                            <b-tab title="ویرایش دسته‌بندی" >
                                <div class="row master-center-content">
                                    <div class="col-1"></div>
                                    <div class="col">
                                        <b-table :fields="categoryFields" :items="categoryItems" sticky-header="200px">
                                        </b-table>
                                    </div>
                                    <div class="col-1"></div>
                                </div>

                                <div class="row master-center-2-content">
                                    <div class="col"></div>
                                    <div class="col-3">
                                        <b-form-select 
                                            v-model="updateCategorySelected" 
                                            :options="categoryOptions"
                                            :disabled="!isAdmin"
                                            ></b-form-select>
                                    </div>
                                    <div class="col-2">
                                        <b-form-input 
                                        type="text" 
                                        v-model="updateCategeryName"
                                        placeholder="نام تغییر یافته دسته‌بندی"
                                        :disabled="!isAdmin"
                                    ></b-form-input>
                                    </div>
                                    <div class="col-1">
                                        <b-button pill @click="editCategory()" :disabled="!isAdmin">تغییر</b-button>
                                    </div>
                                    <div class="col"></div>
                                </div>
                            </b-tab>
                        </b-tabs>
                    </div>


                </div>
                <div v-else-if="isUser" class="col content no-float">
                    <div class="row top-content">
                        <div class="col-1">
                            <img src = "../assets/images/ISNA.jpeg" class = "rounded-circle channel-image" width = "35" height = "35">
                        </div>
                        <div class="col-2 text-right">
                            <h5 class="channel-info">کانال ایسنا</h5>        
                        </div>
                        <div class="col">
                        </div>
                        <div class="col-1">
                            <router-link to="/channel">
                                <font-awesome-icon icon="fa-solid fa-close" class="channel-info-icon close-icon" />
                            </router-link>
                        </div>
                    </div>
                    <div class="row center-content mt-5">
                        <div class="col-6">
                            <b-table :fields="fields" :items="subItems" caption-top>
                            </b-table>
                        </div>
                        <div class="col-6">
                            <div class=""></div>
                            <b-table :fields="categoryFields" :items="categoryItems">
                            </b-table>
                        </div>
                    </div>
                    <div class="row bottom-content mt-5">
                        <div class="col-5">
                            <b-form-select v-model="subBuySelected" :options="subBuyOptions"></b-form-select>
                        </div>
                        <div class="col-1"><b-button pill @click="buySubscription()">خرید</b-button></div>
                        <div class="col"></div>
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
        this.getRole();
    },
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
    data(){
        return {
            isUser: false,
            isAdmin: true,
            addChannelShow: false,
            addSubSelected: null,
            delUserSelected: null,
            addAdmindSelected: null,
            delAdmindSelected: null,
            profitAdminSelected: null,
            incomeAdmin: '',
            subDelSelected: null,
            subBuySelected: null,
            updateCategorySelected: null,
            subSelectedUpdate: null,
            yesStatus: false,
            noStatus:true,
            subFee: '',
            channelImage: '',
            chName: '',
            desName: '',
            subFeeUpdate: '',
            newCategory: '',
            updateCategeryName: '',
            delCategorySelected: null,
            updateLoading: false,
            channelId: this.$route.query.id,
            fields: [
            { key: 'period', label: 'مدت زمان' },
            { key: 'price', label: 'قیمت' },
            ],
            subItems: [] ,
            userFields: [
            { key: 'name', label: 'نام کاربر' },
            ],
            userItems: [] ,
            adminItems:[],
            categoryFields: [
            { key: 'title', label: 'نام دسته‌بندی' },
            ],
            categoryItems: [] ,
            subAddOptionsUpdate: [
                { value: null, text: 'انتخاب نوع حق عضویت' },
            ],
            subAddOptions: [
            { value: null, text: 'انتخاب نوع حق عضویت' },
            { value: 0, text: 'سه ماه' },
            { value: 1, text: 'شش ماه' },
            { value: 2, text: 'دوازده ماه' },
            ],
            subDelOptions: [
            { value: null, text: 'انتخاب نوع حق عضویت' },
            { value: 0, text: 'سه ماه' },
            { value: 1, text: 'شش ماه' },
            { value: 2, text: 'دوازده ماه' },  
            ],
            subBuyOptions: [
            { value: null, text: 'انتخاب نوع حق عضویت' },  
            ],
            percentageOptions:[
            { value: 1, text: '۱۰' },
            { value: 2, text: '۵' },
            { value: 3, text: '۲۰' },
            ],
            userOptions: [
            { value: null, text: 'نام کاربر' },
            ],
            adminOptions: [
            { value: null, text: 'نام مدیر' },
            ],
            categoryOptions: [
            { value: null, text: 'نام دسته‌بندی' },
            ],
            loading: false,
            managersProfit:[],
        }
    },
    methods: {
        getRole(){
            this.loading = true
            let roleApi = "http://79.127.54.112:5000/Channel/GetRole/" + this.$route.query.id
            Vue.axios.get(roleApi, {
                headers: {
                'X-Auth-Token': localStorage.getItem('token')
            }
            })
            .then(response => {
                console.log(response.data.messsage)
                if (response.data.messsage == "MEMBER"){
                    this.isUser = true
                    this.isAdmin = false
                    this.getUserSubscriptions()
                }
                else{
                    this.isUser = false
                    this.isAdmin = true
                    this.getSubscriptions()
                }
            })
        },
        editSubscription(){
            this.loading = true;
            let api = "http://79.127.54.112:5000/Subscription/Edit"
            const data = {
                "SubscriptionId": this.subSelectedUpdate,
                "Price": this.subFeeUpdate
            }
            Vue.axios.put(api, data , {
            headers: {
                'X-Auth-Token': localStorage.getItem('token')
            }
            })
            .then((response) => {
                window.location.reload();
                console.log(response);
            })
            .catch((error) => {
                console.log(error);
                this.$bvToast.toast(error.response.data.message, {title: 'پیام خطا',autoHideDelay: 5000, appendToast: true})
                this.loading = false;
            })

        },
        removeChannel(){
            this.$router.push('/channel');
        },
        goProfile(){
            this.$router.push({name: 'user'})
        },
        editChannel(){
            this.updateLoading = true
            let api = "http://79.127.54.112:5000/Channel/Edit"
            const data = {
                "ChannelId": this.$route.query.id,
                "ChannelName": this.chName,
                "Description": this.desName
            }
            Vue.axios.put(api, data, {
            headers: {
                'X-Auth-Token': localStorage.getItem('token')
            }
            })
            .then(response => {
                this.chName = ''
                this.desName = ''
                this.updateLoading = false
                this.$bvToast.toast(response.data.message, {title: 'پیام',autoHideDelay: 5000, appendToast: true})

            })
            .catch((e) => {
                console.log(e);
                this.chName = ''
                this.desName = ''
                this.updateLoading = true
                this.$bvToast.toast(e.response.data.message, {title: 'پیام خطا',autoHideDelay: 5000, appendToast: true})
            })
        },
        getUsers(){
            let api = "http://79.127.54.112:5000/Channel/GetChannelMembers/" + this.$route.query.id
            Vue.axios.get(api, {
            headers: {
                'X-Auth-Token': localStorage.getItem('token')
            }
            })
            .then((response) => {
                this.userItems = response.data.message;
                for(let i = 0; i < response.data.message.length; i++){
                    if(response.data.message[i].name == null){
                        this.userItems[i].name = 'بی‌نام'
                    }
                }
                for (let i=0; i < response.data.message.length; i++){
                    if (response.data.message[i].name == null){
                        this.userOptions.push({
                        value: response.data.message[i].userId,
                        text: 'بی‌نام'
                    })
                    }
                    else{
                        this.userOptions.push({
                        value: response.data.message[i].userId,
                        text: response.data.message[i].name
                    })
                    }
                }
                this.getCategory();
            })
            .catch((e) => {
                this.$bvToast.toast(e.response.data.message, {title: 'پیام',autoHideDelay: 5000, appendToast: true})
                console.log(e);
                this.loading = false;
            })
        },
        deleteCategory(){
            this.loading = true;
            let api = "http://79.127.54.112:5000/Category/Delete/" + this.delCategorySelected ;
            Vue.axios.delete(api, {
            headers: {
                'X-Auth-Token': localStorage.getItem('token')
            }
            })
            .then(response => {
                console.log(response)
                window.location.reload();
            })
            .catch((e) => {
                console.log(e);
                this.$bvToast.toast(e.response.data.message, {title: 'پیام',autoHideDelay: 5000, appendToast: true})
                this.loading = false;
            })
        },
        getCategory(){
            let api = "http://79.127.54.112:5000/Category/GetAll/" + this.channelId ;
            Vue.axios.get(api, {
            headers: {
                'X-Auth-Token': localStorage.getItem('token')
            }
            })
            .then((response) => {
                console.log(response)
                this.categoryItems = response.data.message
                for (let i=0; i < response.data.message.length; i++){
                    this.categoryOptions.push({
                    value: response.data.message[i].id,
                    text: response.data.message[i].title
                    })
                }
                this.getAdmins();
            })
            .catch((e) => {
                console.log(e);
                this.$bvToast.toast(e.response.data.message, {title: 'پیام',autoHideDelay: 5000, appendToast: true})
                this.loading = false;
            })
        },
        getAdmins(){
            let api = "http://79.127.54.112:5000/Channel/ShowAdmins/" + this.$route.query.id
            Vue.axios.get(api, {
            headers: {
                'X-Auth-Token': localStorage.getItem('token')
            }
            })
            .then(response => {
                this.adminItems = response.data.messsage;
                for (let i=0; i < response.data.messsage.length; i++){
                    this.adminOptions.push({
                        value: response.data.messsage[i].userId,
                        text: response.data.messsage[i].name
                    })
                    this.managersProfit.push({
                        id: response.data.messsage[i].userId,
                        name: response.data.messsage[i].name,
                        profit: ''
                    })

                }
                this.loading = false;
            })
            .catch((e) => {
                console.log(e);
                this.$bvToast.toast(e.response.data.message, {title: 'پیام',autoHideDelay: 5000, appendToast: true})
                this.loading = false;
            })
        
        },
        editCategory(){
            this.loading = true;
            let api= "http://79.127.54.112:5000/Category/Update"
            const data = {
                Id: this.updateCategorySelected,
                Title: this.updateCategeryName
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
                this.$bvToast.toast(e.response.data.message, {title: 'پیام خطا',autoHideDelay: 5000, appendToast: true})
                this.loading = false;
            })
        },
        getUserCategory(){
            let api = "http://79.127.54.112:5000/Category/GetAll/" + this.channelId ;
            Vue.axios.get(api, {
            headers: {
                'X-Auth-Token': localStorage.getItem('token')
            }
            })
            .then((response) => {
                console.log(response)
                this.categoryItems = response.data.message
                for (let i=0; i < response.data.message.length; i++){
                    this.categoryOptions.push({
                    value: response.data.message[i].id,
                    text: response.data.message[i].title
                    })
                }
                this.loading = false;
            })
            .catch((e) => {
                console.log(e);
                this.$bvToast.toast(e.response.data.message, {title: 'پیام',autoHideDelay: 5000, appendToast: true})
                this.loading = false;
            })
        },

        getUserSubscriptions(){
            this.loading = true;
            let api = "http://79.127.54.112:5000/Subscription/" + this.$route.query.id
            Vue.axios.get(api, {
            headers: {
                'X-Auth-Token': localStorage.getItem('token')
            }
            })
            .then(response => {
                this.subItems = response.data.message;
                for (let i = 0; i< response.data.message.length; i++){
                    if(response.data.message[i].period == 0){
                        this.subItems[i].period = 'سه ماه'
                    }
                    else if(response.data.message[i].period == 1){
                        this.subItems[i].period = 'شش ماه'
                    }
                    else if(response.data.message[i].period == 2){
                        this.subItems[i].period = 'دوازده ماه'
                    }
                }
                for(let i = 0; i < response.data.message.length; i++){
                    this.subBuyOptions.push({
                        value: response.data.message[i].id,
                        text: response.data.message[i].period
                    })
                }
                this.getUserCategory()
            })
            .catch((e) => {
                console.log(e);
                this.$bvToast.toast(e.response.data.message, {title: 'پیام',autoHideDelay: 5000, appendToast: true})
                this.loading = false;
            })
        },
        addCategory(){
            this.loading = true;
            let api = "http://79.127.54.112:5000/Category/Add"
            const data = {
                "ChannelId": this.$route.query.id,
                "Title": this.newCategory,
            }
            Vue.axios.post(api, data , {
            headers: {
                'X-Auth-Token': localStorage.getItem('token')
            }
            })
            .then((response) => {
                window.location.reload();
                console.log(response);
            })
            .catch((error) => {
                console.log(error);
                this.$bvToast.toast(error.response.data.message, {title: 'پیام',autoHideDelay: 5000, appendToast: true})
                this.loading = false;
            })

        },
        getSubscriptions(){
            this.loading = true;
            let api = "http://79.127.54.112:5000/Subscription/" + this.$route.query.id
            Vue.axios.get(api, {
            headers: {
                'X-Auth-Token': localStorage.getItem('token')
            }
            })
            .then(response => {
                this.subItems = response.data.message;
                for (let i = 0; i< response.data.message.length; i++){
                    if(response.data.message[i].period == 0){
                        this.subItems[i].period = 'سه ماه'
                    }
                    else if(response.data.message[i].period == 1){
                        this.subItems[i].period = 'شش ماه'
                    }
                    else if(response.data.message[i].period == 2){
                        this.subItems[i].period = 'دوازده ماه'
                    }
                }
                for(let i = 0; i < response.data.message.length; i++){
                    this.subBuyOptions.push({
                        value: response.data.message[i].id,
                        text: response.data.message[i].period
                    })
                    this.subAddOptionsUpdate.push({
                        value: response.data.message[i].id,
                        text: response.data.message[i].period
                    })
                }
                this.getUsers()
            })
            .catch((e) => {
                console.log(e);
                this.$bvToast.toast(e.response.data.message, {title: 'پیام',autoHideDelay: 5000, appendToast: true})
                this.loading = false;
            })
        },
        addSubscription(){
            this.loading = true;
            let api = "http://79.127.54.112:5000/Subscription/AddSubscription"
            const data = {
                "ChannelId": this.$route.query.id,
                "Period": this.addSubSelected,
                "Price": this.subFee
            }
            Vue.axios.post(api, data , {
            headers: {
                'X-Auth-Token': localStorage.getItem('token')
            }
            })
            .then((response) => {
                window.location.reload();
                console.log(response);
            })
            .catch((error) => {
                console.log(error);
                this.$bvToast.toast(error.response.data.message, {title: 'پیام',autoHideDelay: 5000, appendToast: true})
                this.loading = false;
            })
        },
        setIncome(){
            this.loading = true
            let api = "http://79.127.54.112:5000/Channel/SetIncomeShare"
            let incomeDict = {}
            console.log(this.managersProfit)
            for(let i=0; i<this.managersProfit.length; i++){
                incomeDict[this.managersProfit[i].id] = this.managersProfit[i].profit
            }
            const data = {
                "ChannelId": this.$route.query.id,
                "IncomeShares": incomeDict
            }
            Vue.axios.post(api, data , {
            headers: {
                'X-Auth-Token': localStorage.getItem('token')
            }
            })
            .then((response) => {
                console.log(response);
                window.location.reload()
            })
            .catch((error) => {
                this.$bvToast.toast(error.response.data.message, {title: 'پیام',autoHideDelay: 5000, appendToast: true})
                console.log(error);
                this.loading = false;
            })

        },
        addAdmin(){
            this.loading = true;
            let api = "http://79.127.54.112:5000/Channel/AddAdmin"
            const data = {
                "ChannelId": this.$route.query.id,
                "MemberIds": [this.addAdmindSelected]
            }
            Vue.axios.post(api, data , {
            headers: {
                'X-Auth-Token': localStorage.getItem('token')
            }
            })
            .then((response) => {
                console.log(response);
                window.location.reload()
            })
            .catch((error) => {
                this.$bvToast.toast(error.response.data.message, {title: 'پیام',autoHideDelay: 5000, appendToast: true})
                console.log(error);
                this.loading = false;
            })

        },
        removeMember(){
            this.loading = true;
            let api = "http://79.127.54.112:5000/Channel/Member"
            const data = {
                "ChannelId": this.$route.query.id,
                "MemberIds": [this.delUserSelected]
            }
            console.log(localStorage.getItem('token'))
            const headers = {
                'X-Auth-Token': localStorage.getItem('token'),
            }
            Vue.axios.delete(api, {headers: headers, data: data })
            .then(response => {
                console.log(response);
                this.delUserSelected = ''
                window.location.reload()
            })
            .catch((error) => {
                this.$bvToast.toast(error.response.data.message, {title: 'پیام',autoHideDelay: 5000, appendToast: true})
                console.log(error);
                this.loading = false;
            })
        },
        removeAdmin(){
            this.loading = true;
            let api = "http://79.127.54.112:5000/Channel/Admin"
            const data = {
                "ChannelId": this.$route.query.id,
                "AdminIds": [this.delAdmindSelected]
            }
            const headers = {
                'X-Auth-Token': localStorage.getItem('token'),
            }
            Vue.axios.delete(api, {headers: headers, data: data })
            .then((response) => {
                console.log(response);
                window.location.reload()
            })
            .catch((error) => {
                this.$bvToast.toast(error.response.data.message, {title: 'پیام',autoHideDelay: 5000, appendToast: true})
                console.log(error);
                this.loading = false;
            })
        },
        buySubscription(){
            this.loading = true;
            let api = "http://79.127.54.112:5000/Subscription/BuySubscription/" + this.subBuySelected
            Vue.axios.post(api, null , {
            headers: {
                'X-Auth-Token': localStorage.getItem('token')
            }
            })
            .then((response) => {
                console.log(response);
                let channelid  = this.$router.query.id
                this.$router.push({name: 'info-channel', query:{id: channelid}})
                this.loading = false;
            })
            .catch((error) => {
                this.$bvToast.toast(error.response.data.message, {title: 'پیام',autoHideDelay: 5000, appendToast: true})
                console.log(error);
                this.loading = false;
            })
        },
        addImage(){
        this.loading = true;
        let api = "http://79.127.54.112:5000/Channel/AddPicture/"+this.channelId
        let formData = new FormData();
        formData.append('file', this.channelImage);
        Vue.axios.post(api, formData , {
        headers: {
            'X-Auth-Token': localStorage.getItem('token')
        }
        })
        .then((response) => {
            console.log(response);
            let channelid  = this.$router.query.id
            this.channelImage = ''
            this.$router.push({name: 'info-channel', query:{id: channelid}})
            this.loading = false;
        })
        .catch((error) => {
            this.$bvToast.toast(error.response.data.message, {title: 'پیام',autoHideDelay: 5000, appendToast: true})
            console.log(error);
            this.loading = false;
        })
    }
    },



}
</script>
<style>
.nav-link{
    color: black !important;
}

</style>

<style scoped>


.channel-loader{
  position: fixed;
  z-index: 1031;
  top: 50%;
  left: 50%;
}

.image-link, image-link:hover, image-link:visited, image-link:active{
    color: white !important;
    text-decoration: none !important;
}
.close-icon{
    color: black !important;
}
.tabs{
    background-color: #f0f1f2;
    border-radius: 10px;

}
.table{
    background-color: rgb(237, 234, 234) !important;
    border-radius: 20px !important;
}
.b-table-sticky-header{
    background-color: white !important;
    border-radius: 20px !important;
}
.custom-select{
    border-radius: 20px !important;
}
.exit-icon-container{
    bottom:0;
    color: black !important;
}
.exit-icon{
    margin-right: 35px;
    color: black !important;
}
.form-control{
    border-radius: 20px !important;
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
.master-center-content{
    height: 45%;
}
.master-center-2-content{
    height: 15%;
}
.master-center-3-content{
    height: 25%;
}
.master-center-4-content{
    height: 45%;
}
.master-bottom-content{
    height: 15%;
}
.search-icon {
    padding: 0.8rem 0.75rem !important;
    border-top-right-radius: 0px !important;
    border-bottom-right-radius: 0px !important;

}
.top-category-table{
    margin-bottom: 2.5rem;
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
    border-bottom: 1px solid black;
}
.center-content{
    height: 40%;
}
.bottom-content {
    height: 30%;
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
.card-link{
    font-size: 13px;
    color: black;
    text-decoration: none;
}
.card-body{
    text-align: right;
}
.card-link-span{
    float: left;
}
.card{
    border-radius: 20px;
}
.channel-textarea{
    border-radius: 20px;
}
.send-icon{
    margin-top: 40px;
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
caption{
    text-align: center !important;
}
</style>