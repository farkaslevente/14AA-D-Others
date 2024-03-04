import './App.css';
import { Navbar } from './Components/Navbar/Navbar';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import { WelcomePage } from './Pages/WelcomePage';
import { LoginPage } from './Pages/LoginPage';
import { AllAdsPage } from './Pages/AllAdsPage';
import { NewAdPage } from './Pages/NewAdPage';
import { AdDetailsPage } from './Pages/AdDetailsPage';
import { Footer } from './Components/Footer/Footer';
import { RegisterPage } from './Pages/RegisterPage';

function App() {
  return (
    <div>
      <BrowserRouter>
        <Navbar/>

        <Routes>
          <Route path='/' element={<WelcomePage/>}/>
          <Route path='/bejelentkezes' element={<LoginPage/>}/>
          <Route path='/regisztracio' element={<RegisterPage/>}/>
          <Route path='/hirdetesek' element={<AllAdsPage/>}>
            <Route path=':hirdetesId' element={<AdDetailsPage/>}/>
          </Route>
          <Route path='/ujhirdetes' element={<NewAdPage/>}/>

        </Routes>

        <Footer/>
      </BrowserRouter>
    </div>
  );
}

export default App;
