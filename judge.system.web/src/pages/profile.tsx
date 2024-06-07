import React, { useState } from 'react';
import styles from '../styles/Profile.module.css';
import Topbar from '@/components/Topbar/Topbar';
import { Card, Image, Menu, MenuProps } from 'antd';
import { AppstoreOutlined, UserOutlined } from '@ant-design/icons';
import { oneDarkTheme } from '@uiw/react-codemirror';
import SubmitionTable from '@/components/SubmissionTable';



type MenuItem = Required<MenuProps>['items'][number];
const items: MenuItem[] = [
    { key: '1', icon: <UserOutlined />, label: 'Basic Info' },
    { key: '2', icon: <></>, label: 'Account' },
    { key: '3', icon:<></>, label: 'Submission' },
    { key: '4', icon: <></>, label: 'Privacy' },
    { key: '5', icon: <></>, label: 'Account' },
    { key: '6', icon:<></>, label: 'Lab' },
    // {
    //   key: 'sub1',
    //   label: 'Navigation One',
    //   icon: <></>,
    //   children: [
    //     { key: '5', label: 'Option 5' },
    //     { key: '6', label: 'Option 6' },
    //     { key: '7', label: 'Option 7' },
    //     { key: '8', label: 'Option 8' },
    //   ],
    // },
    // {
    //   key: 'sub2',
    //   label: 'Navigation Two',
    //   icon: <></>,
    //   children: [
    //     { key: '9', label: 'Option 9' },
    //     { key: '10', label: 'Option 10' },
    //     {
    //       key: 'sub3',
    //       label: 'Submenu',
    //       children: [
    //         { key: '11', label: 'Option 11' },
    //         { key: '12', label: 'Option 12' },
    //       ],
    //     },
    //   ],
    // },
  ];
  
const ProfilePoints: React.FC = () => {
    const [title ,setTitle] = useState<string>('Basic Info');
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
    <p style={{color:"white",marginLeft:"48px"}}>121232133</p>

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
