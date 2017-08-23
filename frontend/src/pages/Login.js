import React, { Component } from 'react'
import './Login.css'
import axios from 'axios'
import Settings from '../settings.json'

class Login extends Component {
  constructor () {
    super()

    this.state = {
      user: '',
      password: '',
    }
  }

  login () {
    this.clearInfoMessage()

    const e = document.getElementById('info-message')
    if (this.validateFields()) {
      e.innerHTML = '<h5 style="color: red;">Please, fill in both fields.</h5>'
      return
    }

    axios.post(`${Settings.endpoint}${Settings.path.login}`, {
      Username: this.state.user,
      Password: this.state.password,
    })
      .then(res => {
        console.log(res)
        if (res.data === true) {
          this.redirectToMainPage()
        }
      })
      .catch(err => {
        e.innerHTML = '<h5 style="color: red;">Login error. Please, try again.</h5>'
        console.error(err)
      })
  }

  register () {
    this.clearInfoMessage()
    
    const e = document.getElementById('info-message')
    if (this.validateFields()) {
      e.innerHTML = '<h5 style="color: red;">Please, fill in both fields.</h5>'
      return
    }

    axios.post(`${Settings.endpoint}${Settings.path.register}`, {
      Username: this.state.user,
      Password: this.state.password,
    })
      .then(res => {
        console.log(res)
        if (res.data.UserId > 0) {
          e.innerHTML = '<h5>Successfully registered.</h5>'
          
          setTimeout(this.redirectToMainPage, 1000)
        }
      })
      .catch(err => {
        e.innerHTML = '<h5 style="color: red;">Registration error. Please, try again.</h5>'
        console.error(err)
      })
  }

  clearInfoMessage () {
    const e = document.getElementById('info-message')
    e.innerHTML = ''
  }

  validateFields () {
    return this.state.user === '' || this.state.password === ''
  }

  redirectToMainPage () {
    console.log('redirectToMainPage')
  }

  updateValues (field, e) {
    this.setState({
      [field]: e.target.value,
    })
  }

  render () {
    return (
      <div className="Login">
        <div className="Login-header">
          <h2>Personal Accountant</h2>
          <h5>Welcome to the one and only <i>Personal Accountant</i> you need</h5>
        </div>
        <div className="Login-info" id="info-message">
        </div>
        <div className="Login-intro">
          <form className="Login-content">
            <div>
              <label htmlFor="user" className="Login-label">User</label>
              <input type="text" id="user" className="Login-input" value={this.state.user} onChange={e => this.updateValues('user', e)} />
            </div>
            <div>
              <label htmlFor="password" className="Login-label">Password</label>
              <input type="password" id="password" className="Login-input" value={this.state.password} onChange={e => this.updateValues('password', e)} />
            </div>
            <div>
              <input type="button" onClick={() => this.register()} id="register" value="Register" className="Login-button Login-register" />
              <input type="submit" onClick={(e) => {e.preventDefault(); this.login()}} name="submit" id="submit" value="Login" className="Login-button Login-submit" />
            </div>
          </form>
        </div>
      </div>
    )
  }
}

export default Login
