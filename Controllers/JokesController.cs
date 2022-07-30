﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JokesWebApp.Data;
using JokesWebApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace JokesWebApp.Controllers
{
    public class JokesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public JokesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Jokes
        public async Task<IActionResult> Index()
        {
              return _context.Joke != null ? 
                          View(await _context.Joke.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Joke'  is null.");
        }
        // GET: Jokes/ShowSearchForm
        public async Task<IActionResult> ShowSearchForm()
        {
            return View();
        }

        // GET: Jokes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Joke == null)
            {
                return NotFound();
            }

            var joke = await _context.Joke
                .FirstOrDefaultAsync(m => m.id == id);
            if (joke == null)
            {
                return NotFound();
            }

            return View(joke);
        }

        [Authorize]
        // GET: Jokes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Jokes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,jokeQuestion,jokeAnswer")] Joke joke)
        {
            if (ModelState.IsValid)
            {
                _context.Add(joke);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(joke);
        }
        [Authorize]
        // GET: Jokes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Joke == null)
            {
                return NotFound();
            }

            var joke = await _context.Joke.FindAsync(id);
            if (joke == null)
            {
                return NotFound();
            }
            return View(joke);
        }

        // POST: Jokes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,jokeQuestion,jokeAnswer")] Joke joke)
        {
            if (id != joke.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(joke);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JokeExists(joke.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(joke);
        }
        [Authorize]
        // GET: Jokes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Joke == null)
            {
                return NotFound();
            }

            var joke = await _context.Joke
                .FirstOrDefaultAsync(m => m.id == id);
            if (joke == null)
            {
                return NotFound();
            }

            return View(joke);
        }
        [Authorize]
        // POST: Jokes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Joke == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Joke'  is null.");
            }
            var joke = await _context.Joke.FindAsync(id);
            if (joke != null)
            {
                _context.Joke.Remove(joke);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JokeExists(int id)
        {
          return (_context.Joke?.Any(e => e.id == id)).GetValueOrDefault();
        }

        // POST: Jokes/ShowSearchResults
        public async Task<IActionResult> ShowSearchResults(string SearchPhrase)
        {
            return View("Index",await _context.Joke.Where(j=>j.jokeQuestion.Contains(SearchPhrase)).ToListAsync());
        }
    }
}
