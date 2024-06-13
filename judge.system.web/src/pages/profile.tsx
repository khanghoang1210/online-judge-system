import React, { useState } from 'react';
import styles from '../styles/Profile.module.css';
import Topbar from '@/components/Topbar/Topbar';
import { Card, Image, Menu, MenuProps } from 'antd';
import { AppstoreOutlined, LockOutlined, ProfileOutlined, UnorderedListOutlined, UserOutlined } from '@ant-design/icons';
import { oneDarkTheme } from '@uiw/react-codemirror';
import SubmitionTable from '@/components/SubmissionTable';
import { useCookies } from 'react-cookie';
import { jwtDecode } from 'jwt-decode';



type MenuItem = Required<MenuProps>['items'][number];
const items: MenuItem[] = [
    { key: '1', icon: <ProfileOutlined/>, label: 'Basic Info' },
    { key: '2', icon: <UserOutlined/>, label: 'Account' },
    { key: '3', icon:<UnorderedListOutlined/>, label: 'Submission' },
    { key: '4', icon: <LockOutlined/>, label: 'Privacy' }
  ];
  
const ProfilePoints: React.FC = () => {
    const [title ,setTitle] = useState<string>('Basic Info');
    const [cookie] = useCookies(["token"]);
    let user: any = undefined;
    user = jwtDecode(cookie.token)
    const onClick: MenuProps['onClick'] = (e) => {
        const itemCurrent = items.find((item)=>{
            console.log(item)
            if(e.key === item.key)
            {return item;}
        })
       setTitle(itemCurrent.label);
      };
      
  return (
  <>
  <Topbar/>
  {/* <AppstoreOutlined/> */}
  <div style={{backgroundColor:"black",height:"300px",marginBottom:"70px",position:"relative",padding:"150px",display:"flex",alignItems:"center" }}>
    <Image src='/avatar.png' style={{height:150}}/>
    <p style={{color:"white",marginLeft:"48px"}}>{user.nameid}</p>

  <Card title={title} bordered={false} style={{ width:"100%",maxWidth:"calc(100% - 424px)",position:"absolute",top:"200px",left:"400px",height:1000 }}>
    {title ==="Submission" ? <SubmitionTable/>:<></>}
    {title ==="Account" ? <p>Account</p>:<></>}
    {title ==="Privacy" ? <p>Privacy</p>:<></>}
    {title ==="Lab" ? <p>Lab</p>:<></>}
  </Card>
  </div>
  <Menu
      onClick={onClick}
      style={{ width: 256, marginLeft:"80px"}}
      defaultSelectedKeys={['1']}
      defaultOpenKeys={['sub1']}
      mode="inline"

      items={items}
    />
  </>
  );
};

export default ProfilePoints;
