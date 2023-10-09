import Vue from 'vue'
import VueRouter from 'vue-router'
import SignUp from '../views/SignUp.vue'
import SignIn from '../views/SignIn.vue'
import Channel from '../views/Channel.vue'
import AddChannel from '../views/AddChannel.vue'
import InfoChannel from '../views/InfoChannel.vue'
import AddContent from '../views/AddContent.vue'
import EditContent from '../views/EditContent.vue'
import User from '../views/User.vue'

Vue.use(VueRouter)

const routes = [
  {
    path: '/',
    name: 'Signup',
    component: SignUp
  },
  {
    path: '/signin',
    name: 'Signin',
    component: SignIn
  },

  {
    path: '/channel',
    name: 'channel',
    component: Channel
  },
  {
    path: '/add-channel',
    name: 'addchannel',
    component: AddChannel
  },
  {
    path: '/info-channel',
    name: 'infochannel',
    component: InfoChannel
  },
  {
    path: '/add-content',
    name: 'addcontent',
    component: AddContent
  },
  {
    path: '/edit-content',
    name: 'editcontent',
    component: EditContent
  },
  {
    path: '/user',
    name: 'user',
    component: User
  }
]

const router = new VueRouter({
  mode: 'history',
  base: process.env.BASE_URL,
  routes
})

export default router
