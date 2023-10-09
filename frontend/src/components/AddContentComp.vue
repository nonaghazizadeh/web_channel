<template>
    <div>
        <b-spinner class="channel-loader" v-if="totalLoading" label="Spinning"></b-spinner>

        <div v-else dir="rtl" class="channel-page container-fluid">
            <div class="row">
                <div class="col-1 info">
                    <img src = "../assets/images/avatar.png" class = "rounded-circle avatar" width = "40" height = "40" @click="goProfile()">
                    <div class="position-absolute exit-icon-container" >
                        <router-link class="exit-link" to="/">
                            <font-awesome-icon icon="fa-solid fa-arrow-right-from-bracket" class="exit-icon" />
                        </router-link>
                    </div> 
                </div>
                <div class="col content no-float">
                    <div class="row top-content">
                        <div class="col-1">
                            <img src = "../assets/images/channelimg.jpeg" class = "rounded-circle channel-image" width = "35" height = "35">
                        </div>
                        <div class="col-2 text-right">
                            <h5 class="channel-info">{{channelName}}</h5>        
                        </div>
                        <div class="col">
                        </div>
                        <div class="col-1">
                            <font-awesome-icon v-show="!loading" icon="fa-solid fa-close" class="channel-info-icon" @click="close()"/>
                        </div>
                    </div>
                    <div class="row center-add-content">
                        <div class="col">
                            <div class="row center-add-content px-5 py-3 mt-5">
                                <div class="col-6">
                                    <b-form-input type="text" placeholder="عنوان محتوا" v-model="contentTitle"></b-form-input>
                                </div>
                                <div class="col-6">
                                    <b-form-select v-if="editMode" v-model="catSelected" :options="categoryOptions"></b-form-select>
                                </div>
                            </div>
                            <div class="row center-add-content px-5 py-2">
                                <div class="col-6">
                                    <b-form-input type="text" placeholder="توضیحات" v-model="contentDescription"></b-form-input>
                                </div>
                            </div>
                            <div class="row center-2-add-content px-5 py-2">
                                <div class="col-2 py-3">
                                    محتوا به صوت رایگان است؟
                                </div>
                                <div class="col-1 py-3">
                                    <b-form-checkbox
                                    id="checkbox-1"
                                    button-variant="secondary"
                                    v-model="yesStatus"
                                    class="mx-3"
                                    >
                                        بله
                                    </b-form-checkbox>
                                </div>
                                <div class="col-3 py-3">
                                    <b-form-checkbox
                                        id="checkbox-2"
                                        v-model="noStatus"
                                        class="mx-3"
                                    >
                                    خیر
                                    </b-form-checkbox>
                                </div>
                                <div class="col-6">
                                    <b-form-input type="number" placeholder="هزینه محتوا" :disabled="yesStatus" v-model="contentPrice">
                                    </b-form-input>
                                </div>
                            </div>
                            <div class="row center-2-add-content px-5 py-2">
                                <div class="col-2 py-3">
                                    محتوا از کدام یک است؟
                                </div>
                                <div class="col-1 py-3">
                                    <b-form-checkbox
                                    id="checkbox-3"
                                    v-model="isText"
                                    class="mx-1"
                                    >
                                        متن
                                    </b-form-checkbox>
                                </div>
                                <div class="col-1 py-3">
                                    <b-form-checkbox
                                        id="checkbox-4"
                                        v-model="isMusic"
                                        class="mx-1 success"
                                    >
                                        موزیک
                                    </b-form-checkbox>
                                </div>
                                <div class="col-1 py-3">
                                    <b-form-checkbox
                                    id="checkbox-5"
                                    v-model="isVideo"
                                    class="mx-1"
                                    >
                                        ویدیو 
                                    </b-form-checkbox>
                                </div>
                                <div class="col-1 py-3">
                                    <b-form-checkbox
                                    id="checkbox-6"
                                    v-model="isImage"
                                    class="mx-1"
                                    >
                                      عکس
                                    </b-form-checkbox>
                                </div>
                                <div class="col-6">
                                    <b-form-file
                                        class="input-content-file"
                                        v-model="file"
                                        :disabled="!media"
                                        placeholder="" 
                                        @change="handleFileUpload( $event )"
                                    ></b-form-file>
                                </div>
                            </div>
                            <div class="row cetner-3-add-content px-5">
                                <div class="col-12">
                                    <b-form-textarea
                                        id="textarea-rows"
                                        v-model="contentText"
                                        placeholder="متن خود را وارد کنید"
                                        rows="5"
                                        :disabled="!isText"
                                    ></b-form-textarea>  
                                </div>
                            </div>
                            <div class="row bottom-content">
                                <div class="col-10">

                                </div>
                                <div class="col-2">
                                    <b-button v-show="!editMode" variant="secondary" @click="addContent()">
                                        <b-spinner v-if="loading" label="Spinning"></b-spinner>
                                        <span v-else>
                                            افزودن 
                                        </span>
                                    </b-button>
                                    <b-button v-show="editMode" variant="secondary" @click="editContent()">
                                        تغییر     
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
    watch: {
        yesStatus: function (val) {
            if (val) {
                this.noStatus = false
            }
        },
        noStatus: function (val) {
            if (val) {
                this.yesStatus = false
            }
        },
        isText: function (val) {
            if (val) {
                this.isMusic = false
                this.isVideo = false
                this.isImage = false
                this.media = false
            }
        },
        isMusic: function (val) {
            if (val) {
                this.isText = false
                this.isVideo = false
                this.isImage = false
                this.media = true
            }
        },
        isImage: function (val) {
            if (val) {
                this.isText = false
                this.isVideo = false
                this.isMusic = false
                this.media = true
            }
        },
        isVideo: function (val) {
            if (val) {
                this.isText = false
                this.isMusic = false
                this.isImage = false
                this.media = true
            }
        },

    },
    data(){
        return {
            yesStatus: true,
            noStatus: false,
            isText: false,
            isMusic: false,
            isVideo: false,
            isImage: false,
            media: false,
            contentPrice: '',
            editMode: (this.$route.query.edit == "true"),
            channelId: this.$route.query.id,
            contentTitle: '',
            contentDescription:'',
            contentText:'',
            file:'',
            channelName:this.$route.query.name,
            isJoin: true,
            isUser: false,
            modalShow: false,
            addChannelShow: false,
            catSelected: null,
            loading: false,
            totalLoading: false,
            categoryOptions: [
            { value: null, text: 'انتخاب دسته‌بندی' },
            ],
        }
    },

    methods: {
        goProfile(){
            this.$router.push({name: 'user'})
        },
        addContent(){
            this.loading = true;
            let api= "http://79.127.54.112:5000/Content/Add"
            if (this.media){
                let formData = new FormData();
                formData.append('file', this.file);
                formData.append('Title', this.contentTitle);
                formData.append('ChannelId', this.channelId);
                formData.append('Description', this.contentDescription);
                if (this.isMusic){
                    formData.append('Type', 1)
                }
                else if (this.isVideo){
                    formData.append('Type', 2)
                }
                else if (this.isImage){
                    formData.append('Type', 3)
                }
                if (this.yesStatus){
                    this.contentPrice = 0
                }
                formData.append('IsPremium', this.noStatus)
                formData.append('Price', this.contentPrice)

                Vue.axios.post(api, formData,{
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
            else if (this.isText){
                if (this.yesStatus){
                    this.contentPrice = 0
                }
                let formData = new FormData();
                formData.append('Value', this.contentText);
                formData.append('Title', this.contentTitle);
                formData.append('ChannelId', this.channelId);
                formData.append('Description', this.contentDescription);
                formData.append('IsPremium', this.noStatus)
                formData.append('Price', this.contentPrice)
                formData.append('Type', 0)
                Vue.axios.post(api, formData,{
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
        
        },
        editContent(){
            this.$router.push('/channel')
        },
        close(){
            this.$router.push('/channel')
        },
        handleFileUpload(event){
            this.file = event.target.files[0];
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
    text-align-last: left !important;
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
    border-bottom: 1px solid black;
}
.center-add-content{
    height: 25%;
}
.center-2-add-content{
    height: 30%;
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

</style>