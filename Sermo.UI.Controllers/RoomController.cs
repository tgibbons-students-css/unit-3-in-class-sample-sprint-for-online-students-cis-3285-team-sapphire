﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics.Contracts;

using Sermo.UI.Contracts;
using Sermo.UI.ViewModels;
using System.Net;

namespace Sermo.UI.Controllers
{
    public class RoomController : Controller
    {
        public RoomController(IRoomViewModelReader reader, IRoomViewModelWriter writer)
        {
            Contract.Requires<ArgumentNullException>(reader != null);
            Contract.Requires<ArgumentNullException>(writer != null);

            this.reader = reader;
            this.writer = writer;
        }
        
        [HttpGet]
        public ActionResult List()
        {
            var roomListViewModel = new RoomListViewModel(reader.GetAllRooms());
            
            return View(roomListViewModel);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View(new RoomViewModel());
            // Changes Sprint 1 --  I want to create rooms for categorizing conversations -- Tom Gibbons
            // Changes Sprint 1 --  Made another change -- Tom Gibbons
            // Made another change -- Jack Cooper
        }

        [HttpPost]

        // Changes Sprint 1 -- User story 2 -- Jonathan Senger
        // Changes Aprint 1 -- “I want to view a list of rooms that represent conversations.” - Jonathan Senger
        public ActionResult Create(RoomViewModel model)
        {
            ActionResult result;
 
            if(ModelState.IsValid)
            {
                writer.CreateRoom(model);

                result = RedirectToAction("List");
            }
            else
            {
                result = View("Create", model);
            }

            return result;
        }

        [HttpGet]
        public ActionResult Messages(int roomID)
        {
            var messageListViewModel = new MessageListViewModel(reader.GetRoomMessages(roomID));

            return View(messageListViewModel);
        }

        //Sprint 1 #4 - Send plain text messages to other room members
        //Modified By: Jack Seibert
        [HttpPost]
        public ActionResult AddMessage(MessageViewModel messageViewModel)
        {
            ActionResult result;

            if(ModelState.IsValid)
            {
                writer.AddMessage(messageViewModel);

                result = Json(messageViewModel);
            }
            else
            {
                result = new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return result;
        }

        private readonly IRoomViewModelReader reader;
        private readonly IRoomViewModelWriter writer;
    }
}