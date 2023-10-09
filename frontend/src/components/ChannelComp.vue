<template>
    <div>
        <b-spinner class="channel-loader" v-if="channelLoading" label="Spinning"></b-spinner>
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
                <div class="col-2 sidebar no-float">
                    <div class="mt-4 sidebar-top">
                        <span>
                            لیست‌کانال‌ها
                            <font-awesome-icon icon="fa-solid fa-timeline" class="time-icon" @click="showTimeLine()"/>
                            <font-awesome-icon icon="fa-solid fa-magnifying-glass" class="mx-2 search-whole" @click.prevent="openSearchModal()"></font-awesome-icon>
                            <router-link to="/add-channel" class="add-channel-icon">
                                <font-awesome-icon icon="fa-solid fa-plus" class="add-icon"/>
                            </router-link>
                        </span>
                    </div>
                    <div class="input-group mb-3  mt-3">
                        <b-input-group class="mt-3">
                        <template #append>
                        <b-input-group-text>
                            <font-awesome-icon icon="fa-solid fa-magnifying-glass" @click="searchJoin()"/>
                        </b-input-group-text>
                        </template>
                        <b-form-input v-model="searchJoinChannelName"></b-form-input>
                        </b-input-group>
                    </div>
                    <div v-if="joinMode" class="list-group mt-3 w-100 channel-list">
                        <div v-for="channel in searchJoinChannelList" :key="channel.id">
                            <a class="list-group-item list-group-item-action flex-column align-items-start" 
                            :class="{active: activeTag === channel.id }" @click="selectChannel(channel.id, channel)">
                                <div class="d-flex w-100 channel-list-name">
                                    <img src="../assets/images/channelimg.jpeg" class = "rounded-circle" width = "25" height = "25">
                                    <h5 class="mb-1 channel-list-name">{{channel.name}}</h5>
                                </div>
                            </a>
                        </div>
                    </div>
                    <div v-else class="list-group mt-3 w-100 channel-list">
                        <div v-for="channel in channels" :key="channel.id" >
                            <a v-if="channel.isJoin" class="list-group-item list-group-item-action flex-column align-items-start" 
                            :class="{active: activeTag === channel.id }" @click="selectChannel(channel.id, channel)">
                                <div class="d-flex w-100 channel-list-name">
                                    <img src="../assets/images/channelimg.jpeg" class = "rounded-circle ml-3" width = "25" height = "25">
                                    <h5 class="mb-1 channel-list-name ">{{channel.name}}</h5>
                                </div>
                            </a>
                        </div>
                    </div>
                </div>

                <div v-if="activeTag===null && !channelSearchMode && !loadingTimeline" class="col content no-float">
                </div>
                <div v-else-if="contentLoading" class="col content no-float">
                    <b-spinner class="channel-loader" label="Spinning"></b-spinner>
                </div>
                <div v-else-if="loadingTimeline && !channelLoading" class="col content no-float">
                    <div class="col-8">
                        <div v-for="item in timeLineContents" :key="item.contentId">
                            <div class="card mt-3">
                                <div class="card-body">
                                    <div class="mb-3">
                                        <span class="card-title" >
                                                {{item.title}}
                                        </span>
                                    </div>
                                    <p class="card-text">
                                        <span>
                                        {{ item.description }}
                                        </span>
                                        <span  class="card-link-span mr-3">
                                        <a v-show="item.isPremium" class="card-link" @click.prevent="openModal(item.price, item.contentId)">
                                            <font-awesome-icon icon="fa-solid fa-money-check"/>
                                        </a>
                                        </span>
                                        <span class="card-link-show mt-1" @click="showContent(item.contentId)">
                                            <font-awesome-icon icon="fa-solid fa-ellipsis"/>
                                        </span>
                                    </p> 

                                    <span class="card-link-span">
                                        <a class="card-link">{{ new Date(item.createdAt).toLocaleDateString() }}</a>
                                    </span>
                                    <div>
                                        <font-awesome-icon 
                                        icon="fa-solid fa-heart" 
                                        class="mx-3 like-icon" 
                                        :class="{'heart-opacity': !item.isLiked}"
                                        @click="likeDisLike(!item.isLiked, item.contentId)"
                                        />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col"></div>`
                    </div>
                </div>
                <div v-else-if="channelSearchMode && !channelLoading" class="col content no-float">
                    <div class="col-8">
                        <div v-for="item in wholeContents" :key="item.contentId">
                            <div class="card mt-3">
                                <div class="card-body">
                                    <div class="mb-3">
                                        <span class="card-title" >
                                                {{item.title}}
                                        </span>
                                    </div>
                                    <p class="card-text">
                                        <span>
                                        {{ item.description }}
                                        </span>
                                        <span  class="card-link-span mr-3">
                                        <a v-show="item.isPremium" class="card-link" @click.prevent="openModal(item.price, item.contentId)">
                                            <font-awesome-icon icon="fa-solid fa-money-check"/>
                                        </a>
                                        </span>
                                        <span class="card-link-show mt-1" @click="showContent(item.contentId)">
                                            <font-awesome-icon icon="fa-solid fa-ellipsis"/>
                                        </span>
                                    </p> 

                                    <span class="card-link-span">
                                        <a class="card-link">{{ new Date(item.createdAt).toLocaleDateString() }}</a>
                                    </span>
                                    <div>
                                        <font-awesome-icon 
                                        icon="fa-solid fa-heart" 
                                        class="mx-3 like-icon" 
                                        :class="{'heart-opacity': !item.isLiked}"
                                        @click="likeDisLike(!item.isLiked, item.contentId)"
                                        />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col"></div>`
                    </div>
                </div>
                <div v-else-if="activeTag!==null && !contentLoading" class="col content no-float">
                    <div class="row top-content" v-if="!channelSearchMode">
                        <div class="col-1">
                            <img src="../assets/images/channelimg.jpeg" class = "rounded-circle channel-image" width = "35" height = "35">
                        </div>
                        <div class="col-2 text-right">
                            <h5 class="channel-info">{{ currentChannel.name }}</h5>        
                        </div>
                        <div class="col">
                        </div>
                        <div v-show="!currentChannel.isJoin" class="col-1">
                            <font-awesome-icon icon="fa-solid fa-user-plus" class="channel-info-icon" @click="joinChannel(currentChannel.joinLink)"/>
                        </div>               
                        <div v-show="!isUser && currentChannel.isJoin" class="col-1">
                            <font-awesome-icon icon="fa-solid fa-file-circle-plus" class="channel-info-icon" @click="goToAddContent()"/>
                        </div>
                        <div class="col-1">
                            <router-link :to="{name: 'infochannel', query: {id : currentChannel.id}}" class="info-channel-router">
                                <font-awesome-icon icon="fa-solid fa-circle-info" class="channel-info-icon"/>
                            </router-link>
                        </div>
                    </div>
                    <b-input-group class="mt-3" v-if="!channelSearchMode">
                        <template #append>
                        <b-input-group-text>
                            <font-awesome-icon icon="fa-solid fa-magnifying-glass" @click="searchInContents(currentChannel.id)" />
                        </b-input-group-text>
                        </template>
                        <b-form-input v-model="searchContentTitle"></b-form-input>
                    </b-input-group>
                    <div :class="[isUser ? 'row center-content-user':'row center-content']">
                        <div class="col-8">
                            <b-spinner class="channel-loader" v-if="contentLoading" label="Spinning"></b-spinner>
                        </div>
                        <div v-if="contentSearchMode && !contentLoading" class="col-8">
                            <div v-for="item in searchedContentList" :key="item.contentId">
                                <div class="card mt-3">
                                    <div class="card-body">
                                        <div class="mb-3">
                                            <span class="card-title" >
                                                    {{item.title}}
                                            </span>
                                            <span class="edit-icon" v-show="!isUser" @click="goToEditContent(item.contentId)">
                                                <font-awesome-icon icon="fa-solid fa-pen-to-square"/>
                                            </span>
                                            <span class="trash-icon" v-show="!isUser" @click="removeContent(item.contentId)">
                                                <font-awesome-icon icon="fa-solid fa-trash"/>
                                            </span>
                                        </div>
                                        <p class="card-text">
                                            <span>
                                            {{ item.description }}
                                            </span>
                                            <span  class="card-link-span mr-3">
                                            <a v-show="item.isPremium && isUser" lass="card-link" @click.prevent="openModal(item.price, item.contentId)">
                                                <font-awesome-icon icon="fa-solid fa-money-check"/>
                                            </a>
                                            </span>
                                            <span class="card-link-show mt-1" @click="showContent(item.contentId)">
                                                <font-awesome-icon icon="fa-solid fa-ellipsis"/>
                                            </span>
                                        </p> 

                                        <span class="card-link-span">
                                            <a class="card-link">{{ new Date(item.createdAt).toLocaleDateString() }}</a>
                                        </span>
                                        <div>
                                            <font-awesome-icon 
                                            icon="fa-solid fa-heart" 
                                            class="mx-3 like-icon" 
                                            :class="{'heart-opacity': !item.isLiked}"
                                            @click="likeDisLike(!item.isLiked, item.contentId)"
                                            />
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col"></div>
                        </div>
                        <div v-else-if="!contentSearchMode && !contentLoading && !channelSearchMode" class="col-8">
                            <div v-for="item in contents" :key="item.contentId">
                                <div class="card mt-3">
                                    <div class="card-body">
                                        <div class="mb-3">
                                            <span class="card-title" >
                                                    {{item.title}}
                                            </span>
                                            <span class="edit-icon" v-show="!isUser" @click="goToEditContent(item.contentId)">
                                                <font-awesome-icon icon="fa-solid fa-pen-to-square"/>
                                            </span>
                                            <span class="trash-icon" v-show="!isUser" @click="removeContent(item.contentId)">
                                                <font-awesome-icon icon="fa-solid fa-trash"/>
                                            </span>
                                        </div>
                                        <p class="card-text">
                                            <span>
                                            {{ item.description }}
                                            </span>
                                            <span  class="card-link-span mr-3">
                                            <a v-show="item.isPremium && isUser" class="card-link" @click.prevent="openModal(item.price, item.contentId)">
                                                <font-awesome-icon icon="fa-solid fa-money-check"/>
                                            </a>
                                            </span>
                                            <span class="card-link-show mt-1" @click="showContent(item.contentId)">
                                                <font-awesome-icon icon="fa-solid fa-ellipsis"/>
                                            </span>
                                        </p> 
                                        <span class="card-link-span">
                                            <a class="card-link">{{ new Date(item.createdAt).toLocaleDateString() }}</a>
                                        </span>
                                        <div>
                                            <font-awesome-icon 
                                            icon="fa-solid fa-heart" 
                                            class="mx-3 like-icon" 
                                            :class="{'heart-opacity': !item.isLiked}"
                                            @click="likeDisLike(!item.isLiked, item.contentId)"
                                            />
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col"></div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <b-modal dir="rtl" v-model="modalShow" hide-header-close no-close-on-esc no-close-on-backdrop>
            <template #default>
                <p class="modal-text">
                     برای مشاهده کامل محتوا می‌توانید هزینه آن را پرداخت کنید و یا با رفتن به بخش اطلاعات کانال حق‌عضویت خریداری کنید. آیا می‌خواهید این محتوا را بخرید؟ 
                     (هزینه محتوا {{contentPrice}} تومان است)
                </p>
            </template>

            <template #modal-footer="{cancel}">
                <b-button pill size="sm" variant="outline-secondary" @click="ok()">
                    بله، می‌خواهم.          
                </b-button>
                <b-button pill size="sm" variant="outline-dark" @click="cancel()">
                    خیر، نمی‌خواهم.
                </b-button>
            </template>
        </b-modal> 
        <b-modal dir="rtl" v-model="searchModalShow" hide-header-close no-close-on-esc no-close-on-backdrop>
            <template #default>
                <b-form-input type="text" placeholder="عنوان محتوا را برای جست‌وجو در سیستم انتخاب کنید" v-model="searchWholeContentField"></b-form-input>
            </template>

            <template #modal-footer>
                <b-button pill size="sm" variant="outline-secondary" @click="okSearch()">
                    <b-spinner v-if="contentLoading">
                    </b-spinner>
                    <span v-else>
                        جست‌و‌جو   
                    </span>
                </b-button>
                <b-button pill size="sm" variant="outline-dark" @click="cancelSearch()">
                    منصرف شدم.
                </b-button>
            </template>
        </b-modal> 
    </div>
</template>

<script>
import Vue from "vue";
import axios from "axios";
import VueAxios from "vue-axios";
Vue.use(VueAxios, axios);

export default {
    watch:{
        isJoin(){
            this.isNotJoin = !this.isJoin
        },         
        activeTag(){
        for (let i = 0; i < this.channels.length; i++) {
                if (this.channels[i].id === this.activeTag) {
                    this.channelIndex = i
                    break
                }
            }
        },
        searchJoinChannelName(){
            if (this.searchJoinChannelName === ''){
                this.joinMode = false
                this.activeTag = null
            }
        },
        searchContentTitle(){
            if (this.searchContentTitle === ''){
                this.contentSearchMode = false
            }
        }
    },
    created(){
        this.getChannels()
    },

    data(){
        return {
            channelLoading: false,
            contentLoading: false,
            isJoin: true,
            isNotJoin: false,
            isUser: false,
            accessToAddContent: false,
            modalShow: false,
            searchModalShow: false,
            addChannelShow: false,
            activeTag: null,
            channelIndex: null,
            searchJoinChannelName: '',
            searchContentTitle: '',
            searchedChannelList: [],
            searchJoinChannelList: [],
            searchedContentList: [],
            channelSearchMode: false,
            contentSearchMode: false,
            channels: [],
            allChannels: [],
            contents: [],
            contentPrice: 0,
            wholeContents: [],
            timeLineContents: [],
            contentId: null,
            searchWholeContentField: '',
            notLike: true,
            joinMode: false,
            loadingTimeline: false,
            currentChannel: {
                id: null,
                name: null,
                isJoin: null,
                joinLink: null,
            },
        }
    },
    methods: {
        openModal(price, id) {
            this.modalShow = true
            this.contentPrice = price;
            this.contentId = id;
        },
        openSearchModal(){
            this.searchModalShow = true
        },
        cancel() {
            this.modalShow = false
        },
        cancelSearch() {
            this.searchModalShow = false
        },
        okSearch(){
            this.channelLoading = true
            this.contentLoading = true
            this.activeTag = null
            let api = "http://79.127.54.112:5000/Search?title="+ this.searchWholeContentField;
            Vue.axios.get(api, {
            headers: {
                'X-Auth-Token': localStorage.getItem('token')
            }
            })
            .then(response => {
                this.wholeContents = response.data.message
                this.channelSearchMode = true;
                this.channelLoading = false;
                this.contentLoading= false;
                this.searchModalShow = false;
            })
            .catch((e) => {
                this.channelLoading = false
                this.contentLoading = false
                this.$bvToast.toast(e.response.data.message, {title: 'پیام خطا',autoHideDelay: 5000, appendToast: true})

            })
        },
        ok(){
            let api = "http://79.127.54.112:5000/Subscription/BuyContent/" + this.contentId
            Vue.axios.post(api, null, {
                headers: {
                    'X-Auth-Token': localStorage.getItem('token')
                }
            })
            .then(response => {
                console.log(response)
                window.location.reload()
            })
            .catch(error => {
                console.log(error)
                this.modalShow = false
                this.$bvToast.toast(error.response.data.message, {title: 'پیام خطا',autoHideDelay: 5000, appendToast: true})
            })
        },
        likeDisLike(notLike, id){
            if (notLike){
                this.like(id)
            }
            else if(!notLike){
                this.dislike(id)
            }
        },
        like(id){
            let api = "http://79.127.54.112:5000/Content/Like/"+id
            Vue.axios.post(api, null, {
                headers: {
                    'X-Auth-Token': localStorage.getItem('token')
                }
            })
            .then(response => {
                console.log(response)
                window.location.reload()
            })
            .catch(error => {
                console.log(error)
                this.modalShow = false
                this.$bvToast.toast(error.response.data.message, {title: 'پیام خطا',autoHideDelay: 5000, appendToast: true})
            })
        },
        dislike(id){
            let api = "http://79.127.54.112:5000/Content/UnLike/"+id
            const data = null
            const headers = {
                'X-Auth-Token': localStorage.getItem('token'),
            }
            Vue.axios.delete(api, {headers: headers, data: data })
            .then(response => {
                console.log(response)
                window.location.reload()
            })
            .catch(error => {
                console.log(error)
                this.modalShow = false
                this.$bvToast.toast(error.response.data.message, {title: 'پیام خطا',autoHideDelay: 5000, appendToast: true})
            })
        },
        showTimeLine(){
            this.activeTag = null
            this.channelLoading = true
            this.contentLoading = true
            let api = "http://79.127.54.112:5000/Timeline/Load";
            Vue.axios.get(api, {
            headers: {
                'X-Auth-Token': localStorage.getItem('token')
            }
            })
            .then(response => {
                console.log(response)
                this.timeLineContents = response.data.message
                this.loadingTimeline = true;
                this.channelLoading = false
                this.contentLoading = false
            })
            .catch((e) =>{
                this.$bvToast.toast(e.response.data.message, {title: 'پیام خطا',autoHideDelay: 5000, appendToast: true})
                this.channelLoading = false
                this.contentLoading = false
            })
        },
        searchJoin(){
            this.searchJoinChannelList = []
            for (let i = 0; i < this.allChannels.length; i++) {
                if (this.allChannels[i].name === this.searchJoinChannelName) {
                    this.searchJoinChannelList.push(this.allChannels[i])   
                }
            }
            this.joinMode = true;
        },
        showContent(id){
            let api = "http://79.127.54.112:5000/Content/ShowContent/" + id
            const headers = {
                'X-Auth-Token': localStorage.getItem('token')
            }
            Vue.axios.get(api, {
                headers: headers
            })
            .then(response => {
                if(response.data.message.contentType === 'Text'){
                    console.log(response.data.message.value)
                    this.$bvModal.msgBoxOk(response.data.message.value, {
                        title: 'محتوای متنی',
                        size: 'lg',
                        buttonSize: 'sm',
                        okVariant: 'secondary',
                        headerClass: 'p-2 border-bottom-0',
                        footerClass: 'p-2 border-top-0',
                        centered: true,
                        okTitle: 'بستن',
                        modalClass: 'my-modal'

                    })
                }
                else{
                    window.open("http://79.127.54.112:5000/Contents/"+response.data.message.value, '_blank')
                }
            })
            .catch(error => {
                this.$bvToast.toast(error.response.data.message, {title: 'پیام خطا',autoHideDelay: 5000, appendToast: true})
                console.log(error)
            })
        },
        selectChannel(id, cchannel){
            this.loadingTimeline = false
            this.channelSearchMode = false
            this.contentLoading = true
            this.activeTag = id
            this.currentChannel.id = id
            this.currentChannel.name = cchannel.name
            this.currentChannel.joinLink = cchannel.joinLink
            this.currentChannel.isJoin = cchannel.isJoin
            let api = 'http://79.127.54.112:5000/Content/GetContentsMetaData/' + id
            Vue.axios.get(api, {
            headers: {
                'X-Auth-Token': localStorage.getItem('token')
            }
            })
            .then(response => {
                console.log(response)
                this.contents = response.data.message;
                let roleApi = "http://79.127.54.112:5000/Channel/GetRole/" + id
                Vue.axios.get(roleApi, {
                    headers: {
                    'X-Auth-Token': localStorage.getItem('token')
                }
                })
                .then(response => {
                    console.log(response.data.messsage)
                    if (response.data.messsage == "MEMBER"){
                        this.isUser = true
                    }
                    else {
                        this.isUser = false
                    }
                    this.contentLoading = false
                })
                .catch(error => {
                    console.log(error)
                    this.$bvToast.toast(error.response.data.message, {title: 'پیام خطا',autoHideDelay: 5000, appendToast: true})
                    this.contentLoading = false
                })
            }) 
            .catch(error => {
                console.log(error)
                this.$bvToast.toast(error.response.data.message, {title: 'پیام خطا',autoHideDelay: 5000, appendToast: true})
                this.contentLoading = false
            })
        },
        removeFromChannel(channelId){
            this.isJoin = false
            for (let i = 0; i < this.channels.length; i++) {
                if (this.channels[i].id === channelId) {
                    this.channels[i].isJoin = false
                    this.channels.splice(i, 1)
                    break
                }
            }
            this.activeTag = null 
        },
        removeContent(dataId){
            this.contentLoading = true
            let api = "http://79.127.54.112:5000/Content/RemoveContent/" + dataId
            Vue.axios.delete(api, {
            headers: {
                'X-Auth-Token': localStorage.getItem('token')
            }
            })
            .then(response => {
                console.log(response)
                setTimeout(() => {
                    window.location.reload()
                }, 2000);
            })
            .catch(error => {
                this.$bvToast.toast(error.response.data.message, {title: 'پیام خطا',autoHideDelay: 5000, appendToast: true})
                console.log(error)
                this.contentLoading = true
            })
        },
        goProfile(){
            this.$router.push({name: 'user'})
        },
        goToAddContent(){
            this.$router.push({name: 'addcontent', query:{edit: false, id: this.channels[this.channelIndex].id, name: this.channels[this.channelIndex].name}})
        },
        goToEditContent(id){
            this.$router.push({name: 'editcontent', query:{id: this.channels[this.channelIndex].id, name: this.channels[this.channelIndex].name, contentId: id}})
        },
        searchInChannels(){
            this.channelLoading = true
            this.contentLoading = true
            let api = "http://79.127.54.112:5000/Search?title="+ this.searchedChannelName;
            Vue.axios.get(api, {
            headers: {
                'X-Auth-Token': localStorage.getItem('token')
            }
            })
            .then(response => {
                console.log(response)
                this.wholeContents = response.data.message
                this.channelSearchMode = true;
                this.channelLoading = false;
                this.contentLoading= false;
            })
            .catch((e) =>{
                this.$bvToast.toast(e.response.data.message, {title: 'پیام خطا',autoHideDelay: 5000, appendToast: true})
                this.channelLoading = false;
                this.contentLoading= false;
            })
        },
        searchInContents(id){
            this.contentLoading = true
            let api = "http://79.127.54.112:5000/Search/" + id + "?title="+ this.searchContentTitle;
            Vue.axios.get(api, {
            headers: {
                'X-Auth-Token': localStorage.getItem('token')
            }
            })
            .then(response => {
                console.log(response)
                this.searchedContentList = response.data.message;
                this.contentSearchMode = true;
                this.contentLoading = false;
            })
            .catch((e) =>{
                this.$bvToast.toast(e.response.data.message, {title: 'پیام خطا',autoHideDelay: 5000, appendToast: true})
                this.contentSearchMode = false;
                this.contentLoading = false;
            })
        },
        getChannels(){
            this.channelLoading = true
            let api = "http://79.127.54.112:5000/Channel/GetJoinedChannels";
            Vue.axios.get(api, {
            headers: {
                'X-Auth-Token': localStorage.getItem('token')
            }
            })
            .then(response => {
                console.log(response)
                this.channels = response.data.message;
                for (let i = 0; i < this.channels.length; i++) {
                    this.channels[i].isJoin = true
                }
                let allChannlesApi = "http://79.127.54.112:5000/Channel/AllChannels";
                Vue.axios.get(allChannlesApi, {
                headers: {
                    'X-Auth-Token': localStorage.getItem('token')
                }
                })
                .then(response => {
                    this.allChannels = response.data.message;
                    for(let i = 0; i < this.allChannels.length; i++){
                        for(let j = 0; j < this.channels.length; j++){
                            if(this.allChannels[i].id === this.channels[j].id){
                                this.allChannels[i].isJoin = true
                            }
                            else{
                                this.allChannels[i].isJoin = false
                            }
                        }
                    }
                    this.channelLoading = false;
                })
                .catch((e) => {
                    console.log(e)
                    this.$bvToast.toast(e.response.data.message, {title: 'پیام خطا',autoHideDelay: 5000, appendToast: true})
                    this.channelLoading = false
                })
            })
            .catch((e) => {
                console.log(e)
                this.$bvToast.toast(e.response.data.message, {title: 'پیام خطا',autoHideDelay: 5000, appendToast: true})
                this.channelLoading = false
            })
        },
        joinChannel(joinLink){
            let api = "http://79.127.54.112:5000/Channel/Join/" + joinLink
            this.channelLoading = true
            Vue.axios.post(api, null, {
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
                this.channelLoading = false
                this.$bvToast.toast(e.response.data.message, {title: 'پیام خطا',autoHideDelay: 5000, appendToast: true})
            })

        }
    }
}
</script>

<style scoped>
.time-icon{
    margin-right: 60px;
    cursor: pointer;
}
.edit-icon, .search-whole{
    cursor: pointer;
}
.like-icon{
    cursor: pointer;
}
.heart-opacity{
    opacity: 30%;
}
.channel-loader{
  position: fixed;
  z-index: 1031;
  top: 50%;
  left: 50%;
}
.info-channel-router{
    color: black !important;
}
.add-channel-icon{
    color:black !important;
}
.exit-link{
    color: black !important;
}

.fa-disabled {
  opacity: 0.6;
  cursor: not-allowed;
}
.modal-text{
    text-align: right !important;
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
    height: 100vh;
}
.sidebar{
    height: 100vh;
    overflow-y: scroll;
    background-color: white;
}
.channel-list{
    height: 100vh;
    overflow-y: scroll;
}
.content{
    background-color: rgb(226, 226, 226);
    height: 100vh;
    overflow-y: scroll;
}
.row{
    height: 100%;
}
.card-text a{
    color:black !important;
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
    height: 80px;
    background-color: white;
}
.center-content{
    height: 70%;
}
.center-content-user{
    height: 90%;
}
.bottom-content {
    height: 20%;

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
.channel-info-icon:hover{
    cursor: pointer;
}
.card-link{
    font-size: 13px;
    color: black;
    text-decoration: none;
    text-align: left !important;
    direction: ltr !important;
}
.card-body{
    text-align: right;
}
.card-link-span{
    float: left;
}
.card-link-show, .card-link-show:hover{
    float:left;
    font-size: 10px;
    color: black;
    text-decoration: none;
    cursor: pointer;
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
    font-size: 15px;
    text-align: right;
    margin-right: 5px;
    font-weight: bold;
}
.add-icon:hover{
    cursor: pointer;
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
.card-title{
    font-size: 22px;
    font-weight: bold;
}
.trash-icon{
    margin-right: 5px;
    cursor: pointer;
}
.channel-list-name{
    text-align: right !important;
    font-size: 15px;
}

</style>
<style>
.my-modal{
    direction: rtl !important;
    left: auto;
    right: 0;
}
.my-modal .modal-body{
    text-align: right !important;
}
</style>